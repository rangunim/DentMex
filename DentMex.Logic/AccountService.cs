using DentMex.Domain;
using DentMex.Logic.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DentMex.Logic
{
    public class AccountService : IAccountService
    {
        // odwolania do tabel bazy danych
        private readonly IRepository<City> CityRepository;
        private readonly IRepository<Gender> GenderRepository;
        private readonly IRepository<UserRole> UserRoleRepository;
        private readonly IRepository<Account> AccountRepository;
      
        public AccountService(IRepository<City> CityRepository, IRepository<Gender> GenderRepository,
            IRepository<UserRole> UserRoleRepository, IRepository<Account> AccountRepository)
        {
            this.CityRepository = CityRepository;
            this.GenderRepository = GenderRepository;
            this.UserRoleRepository = UserRoleRepository;
            this.AccountRepository = AccountRepository;
        }

        public ICollection<City> GetCities()
        {
            return CityRepository.GetAll().ToList();
        }
        public ICollection<Gender> GetGenders()
        {
            return GenderRepository.GetAll().ToList();
        }

        public ICollection<UserRole> GetUserRoles()
        {
            return UserRoleRepository.GetAll().ToList();
        }

        public ICollection<Account> GetAccounts()
        {
            return AccountRepository.GetAll().ToList();
        }


        private string GetMD5(string text)
        {
            byte[] textByte = Encoding.UTF8.GetBytes(text);
            byte[] hashed = null;
            using (MD5 hasher = MD5.Create())
            {
                hashed = hasher.ComputeHash(textByte);
            }
            StringBuilder result = new StringBuilder(hashed.Length * 2);
            for (int i = 0; i < hashed.Length; i++)
            {
                result.Append(hashed[i].ToString("X2"));
            }
            return result.ToString();
        }

        public Account GetAccountByUsernameAndPassword(string username, string password) //do logowania
        {
            string md5pass = GetMD5(password);
            Account result = AccountRepository.FindBy(x => x.AccountLogin.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                x.AccountPassword.Equals(md5pass));
            return result;
        }

        public Account GetAccountById(int id)
        {
            return AccountRepository.GetAll().FirstOrDefault(x => x.AccountId == id);
        }

        public Account GetAccountByLogin(string login)
        {
            return AccountRepository.FindBy(x => x.AccountLogin.Equals(login, StringComparison.OrdinalIgnoreCase)); 
        }

        public List<Account> GetAccountsByUserRole(string userRoleName)
        {
            var role = UserRoleRepository.FindBy(x => x.RoleName.Equals(userRoleName));
            if(role == null)
            {
                return null;
            }
            var accounts = AccountRepository.GetBy(x => x.UserRoleId == role.UserRoleId);
            return accounts.ToList();
        }
        public UserRole GetUserRole(string login)
        {
            var user = AccountRepository.FindBy(x => x.AccountLogin.Equals(login));
            if(user == null)
            {
                return null;
            }
            var role = UserRoleRepository.FindBy(x => x.UserRoleId == user.UserRoleId);
            return role;
        }

        public City GetCity(string login)
        {
            var user = AccountRepository.FindBy(x => x.AccountLogin.Equals(login));
            if(user == null)
            {
                return null;
            }
            var city = CityRepository.FindBy(x => x.CityId == user.CityId);
            return city;
        }

        public Gender GetGender(string login)
        {
            var user = AccountRepository.FindBy(x => x.AccountLogin == login);
            if(user == null)
            {
                return null;
            }
            var gender = GenderRepository.FindBy(x => x.GenderId == user.GenderId);
            return gender;
        }

        public bool RegisterAccount(Account account)
        {
            if(!ValidateAccount(account))
                return false;

            // sprawdzenie, czy imie i nazwisko zawieraja jedynie litery, spacje i myslnik
            if(account.FirstName.Any(x => !Char.IsLetter(x) && x != ' ' && x != '-'))
                return false;
            if(account.LastName.Any(x => !Char.IsLetter(x) && x != ' ' && x != '-'))
                return false;

            // sprawdzenie, czy jest co najmniej jedna cyfra i co najmniej jedna litera
            Regex rgx = new Regex(@"(([0-9]).*([a-zA-z]).*)|(([a-zA-z]).*([0-9]).*)");
            if(!rgx.IsMatch(account.AccountPassword))
                return false;

            // czy login lub email istnieja w bazie
            var accountList = GetAccounts().ToList();        
            if(accountList.FirstOrDefault(x => x.AccountLogin.Equals(account.AccountLogin,StringComparison.OrdinalIgnoreCase) || x.Email.Equals(account.Email,StringComparison.OrdinalIgnoreCase)) != null)
                return false;

            if(account.AccountPassword.Length < 6)
                return false;

            account.AccountPassword = GetMD5(account.AccountPassword);
            AccountRepository.Add(account);
            return true;
        }

        private bool ValidateAccount(Account account)
        {
            if(account == null)
                return false;

            // sprawdzenie poprawnosci maila
            if(!(new EmailAddressAttribute().IsValid(account.Email)))
                return false;

            return true;
        }

        public bool EditAccount(Account newDataAccount)
        {
            if(!ValidateAccount(newDataAccount))
                return false;

            // czy login istnieje w bazie
            Account editedAccount = AccountRepository.FindBy(x => x.AccountId == newDataAccount.AccountId);

            if(editedAccount == null)
                return false;

            var accountList = GetAccounts().ToList();
            if(!editedAccount.Email.Equals(newDataAccount.Email, StringComparison.OrdinalIgnoreCase))
            {
                if(accountList.FirstOrDefault(x => x.Email == newDataAccount.Email) != null)
                    return false;
            }

            if(newDataAccount.AccountPassword != null && newDataAccount.AccountPassword.Length >= 6)
            {
                string password = GetMD5(newDataAccount.AccountPassword);
                editedAccount.AccountPassword = password;
            }

            
            editedAccount.Email = newDataAccount.Email;
            editedAccount.InsuranceNumber = newDataAccount.InsuranceNumber;
            editedAccount.Address = newDataAccount.Address;
            editedAccount.PostalCode = newDataAccount.PostalCode;
            editedAccount.CityId = newDataAccount.CityId;
            editedAccount.Phone = newDataAccount.Phone;

            AccountRepository.Update(editedAccount);
            return true;
        }

        public bool DeleteAccount(string login)
        {
            var user = AccountRepository.FindBy(x => x.AccountLogin.Equals(login));
            if(user != null)
            {
                AccountRepository.Delete(user);
                return true;
            }
            return false;
        }

        public bool ChangeUserRole(string accountLogin, short userRoleId)
        {
            var account = AccountRepository.FindBy(x => x.AccountLogin.Equals(accountLogin));
            if(account == null)
            {
                return false;
            }
            account.UserRoleId = userRoleId;
            try
            {
                AccountRepository.Update(account);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
