using System.ComponentModel.DataAnnotations;

namespace testTOKEN.Model
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        private List<Account> _accountList;
        public List<Account> AccountList
        {
            get { return _accountList; }
            set { _accountList = value; }
        }
        public List<Account> AddAccount()
        {
            List<Account> listAcc = new List<Account>();
            listAcc.Add(new Account { Id = 1, account = "Nguyễn Văn A", password = "abc" });
            listAcc.Add(new Account { Id = 2, account = "Nguyễn Văn B", password = "abc" });
            listAcc.Add(new Account { Id = 3, account = "Nguyễn Văn C", password = "abc" });
            return listAcc;
        }
    }
}
