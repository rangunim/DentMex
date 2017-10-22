using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentMex.Domain;

namespace DentMex.Logic
{
    public interface IAccountService
    {
        ICollection<City> GetCities();
        ICollection<Gender> GetGenders();
        ICollection<UserRole> GetUserRoles();
        ICollection<Account> GetAccounts();

        Account GetAccountByUsernameAndPassword(string login, string password);
        Account GetAccountById(int id);
        Account GetAccountByLogin(string login);
        List<Account> GetAccountsByUserRole(string userRoleName);

        UserRole GetUserRole(string login);
        City GetCity(string login);
        Gender GetGender(string login);

        bool RegisterAccount(Account account);
        bool EditAccount(Account newDataAccount);
        bool DeleteAccount(string login);
        bool ChangeUserRole(string accountLogin, short userRoleId);
    }
}
