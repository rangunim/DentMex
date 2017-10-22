using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentMex.Logic;
using DentMex.Logic.Repository;
using DentMex.Domain;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DentMex.UnitTests
{
    [TestClass]
    public class AccountServiceTests
    {
        // listy z danymi
        private List<City> CityList;
        private List<Gender> GenderList;
        private List<UserRole> UserRoleList;
        private List<Account> AccountList;
        // mockupy tabel
        private Mock<IRepository<City>> MockCityRepository;
        private Mock<IRepository<Gender>> MockGenderRepository;
        private Mock<IRepository<UserRole>> MockUserRoleRepository;
        private Mock<IRepository<Account>> MockAccountRepository;
        // referencja do serwisu
        private IAccountService Target;

        [TestInitialize]
        public void TestInitialize()
        {
            #region City
            // przykładowe dane miast
            CityList = new List<City>
            {
                new City(){ CityId = 1, CityName = "Testowe" },
                new City(){ CityId = 2, CityName = "Wrocław" }
            };
            // przykładowy mock tabeli miast - piszemy w nim definicje metod które mogą być wykorzystywane
            // mimo że do miast potrzebne jest tylko GetAll, to piszę pozostałe aby był wzór do reszty
            MockCityRepository = new Mock<IRepository<City>>();
            MockCityRepository.Setup(m => m.GetAll()).Returns(CityList.AsQueryable());
            MockCityRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<City, bool>>>(), It.IsAny<Expression<Func<City, object>>[]>()))
                .Returns<Expression<Func<City, bool>>, Expression<Func<City, object>>[]>((predicate, includeProperties)
                    => CityList.AsQueryable().FirstOrDefault(predicate));
            MockCityRepository.Setup(m => m.Update(It.IsAny<City>())).Callback((City city) =>
            {
                int i = CityList.FindIndex(x => x.CityId == city.CityId);
                CityList[i] = city;
            });
            MockCityRepository.Setup(m => m.Delete(It.IsAny<City>())).Callback((City city) =>
            {
                CityList.Remove(city);
            });
            MockCityRepository.Setup(m => m.Add(It.IsAny<City>())).Callback((City city) =>
            {
                city.CityId = (short)(CityList.Last().CityId + 1);
                CityList.Add(city);
            });
            #endregion
            #region Gender
            GenderList = new List<Gender> 
            {
                new Gender(){ GenderId=1, GenderName="Test" }
            };
            MockGenderRepository = new Mock<IRepository<Gender>>();
            MockGenderRepository.Setup(m => m.GetAll()).Returns(GenderList.AsQueryable());
            MockGenderRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<Gender, bool>>>(), It.IsAny<Expression<Func<Gender, object>>[]>()))
                .Returns<Expression<Func<Gender, bool>>, Expression<Func<Gender, object>>[]>((predicate, includeProperties)
                    => GenderList.AsQueryable().FirstOrDefault(predicate));
            #endregion
            #region UserRole
            UserRoleList = new List<UserRole>
            {
                new UserRole(){ UserRoleId=1, RoleName="Admin" }
            };
            MockUserRoleRepository = new Mock<IRepository<UserRole>>();
            MockUserRoleRepository.Setup(m => m.GetAll()).Returns(UserRoleList.AsQueryable());
            MockUserRoleRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<UserRole, bool>>>(), It.IsAny<Expression<Func<UserRole, object>>[]>()))
                .Returns<Expression<Func<UserRole, bool>>, Expression<Func<UserRole, object>>[]>((predicate, includeProperties)
                    => UserRoleList.AsQueryable().FirstOrDefault(predicate));
            #endregion
            #region Account
            AccountList = new List<Account>
            {
                new Account()
                { 
                    AccountLogin="logins", 
                    AccountPassword="098f6bcd4621d373cade4e832627b4f6",
                    CityId=2,
                    City=CityList[1],
                    Email="e@e-mail.com",
                    FirstName="Imie",
                    GenderId=1,
                    Gender=GenderList[0],
                    LastName="Nazwisko",
                    UserRoleId=1,
                    UserRole=UserRoleList[0]
                }
            };
            MockAccountRepository = new Mock<IRepository<Account>>();
            MockAccountRepository.Setup(m => m.GetAll()).Returns(AccountList.AsQueryable());
            MockAccountRepository.Setup(m => m.FindBy(It.IsAny<Expression<Func<Account, bool>>>(), It.IsAny<Expression<Func<Account, object>>[]>()))
                .Returns<Expression<Func<Account, bool>>, Expression<Func<Account, object>>[]>((predicate, includeProperties)
                    => AccountList.AsQueryable().FirstOrDefault(predicate));
            MockAccountRepository.Setup(m => m.Update(It.IsAny<Account>())).Callback((Account account) =>
            {
                int i = AccountList.FindIndex(x => x.AccountLogin == account.AccountLogin);
                AccountList[i] = account;
            });
            MockAccountRepository.Setup(m => m.Delete(It.IsAny<Account>())).Callback((Account account) =>
            {
                AccountList.Remove(account);
            });
            MockAccountRepository.Setup(m => m.Add(It.IsAny<Account>())).Callback((Account account) =>
            {
                account.AccountLogin = (AccountList.Last().AccountLogin);
                AccountList.Add(account);
            });
            #endregion
            Target = new AccountService(MockCityRepository.Object, MockGenderRepository.Object, MockUserRoleRepository.Object,
                MockAccountRepository.Object);
        }

        [TestMethod]
        public void CanGetCities()
        {
            var result = Target.GetCities();
            Assert.IsNotNull(result);
            Assert.AreEqual(CityList.Count, result.Count);
            Assert.AreEqual("Testowe",result.ToList()[0].CityName);
        }

        [TestMethod]
        public void CanGetAccounts()
        {
            var result = Target.GetAccounts();
            Assert.IsNotNull(result);
            Assert.AreEqual(AccountList.Count, result.Count);
            Assert.AreEqual("logins", result.ToList()[0].AccountLogin);
        }

        [TestMethod]
        public void CanGetAccount()
        {
            var result = Target.GetAccountByLogin("logins");
            Assert.IsNotNull(result);
            Assert.AreEqual(AccountList[0], result);
        }

        [TestMethod]
        public void CanRegisterAccount()
        {
            var input = new Account()
            {
                AccountLogin = "Michal",
                AccountPassword = "Michal_test_04.",
                Email = "a@a.pl",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber="429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode="54-033",
                Phone="777777",
                
                CityId = 1,
                UserRoleId = 1
            };
            string pass = "Michal_test_04.";

            var result = Target.RegisterAccount(input);
            Assert.IsTrue(result);
            Assert.AreEqual(AccountList[AccountList.Count - 1].AccountLogin, input.AccountLogin);
            Assert.AreEqual(AccountList[AccountList.Count - 1].AccountPassword, input.AccountPassword);

            var loginResult = Target.GetAccountByUsernameAndPassword(input.AccountLogin, pass);
            Assert.IsNotNull(loginResult);
            Assert.AreEqual(input.AccountLogin, loginResult.AccountLogin);
            // bledny email - niepoprawny format
            input = new Account()
            {
                AccountLogin = "Michal2",
                AccountPassword = "Michal_test_04.",
                Email = "a@",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(result);
            // bledne haslo - mniej niz 6 znakow
            input = new Account()
            {
                AccountLogin = "Michal3",
                AccountPassword = "ag4Ci",
                Email = "a@a2.pl",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(result);
            // bledne haslo - brak co najmniej jednej cyfry
            input = new Account()
            {
                AccountLogin = "Michal5",
                AccountPassword = "Michaltest",
                Email = "a@a4.pl",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(result);
            // bledne haslo - brak co najmniej jednej litery
            input = new Account()
            {
                AccountLogin = "Michal6",
                AccountPassword = "45.39-36",
                Email = "a@a5.pl",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(result);
            // bledny login - powtarzajacy sie
            input = new Account()
            {
                AccountLogin = "login",
                AccountPassword = "Michal_test_04.",
                Email = "a@a3.pl",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(!result);
            // bledny email - powtarzajacy sie
            input = new Account()
            {
                AccountLogin = "Michal4",
                AccountPassword = "Michal_test_04.",
                Email = "e@e-mail.com",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(result);
            // bledne imie
            input = new Account()
            {
                AccountLogin = "Michal7",
                AccountPassword = "Michal_test_04.",
                Email = "e@e2-mail.com",
                FirstName = "test@",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(result);
            // bledne nazwisko
            input = new Account()
            {
                AccountLogin = "Michal8",
                AccountPassword = "Michal_test_04.",
                Email = "e@e3-mail.com",
                FirstName = "testo wy",
                LastName = "testowy-testowy",
                InsuranceNumber = "429429",
                Pesel = "534345",
                Address = "fsfs",
                PostalCode = "54-033",
                Phone = "777777",
                CityId = 1,
                UserRoleId = 1
            };
            result = Target.RegisterAccount(input);
            Assert.IsFalse(!result);
        }
    }
}
