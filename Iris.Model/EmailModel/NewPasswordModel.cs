namespace Iris.Model.EmailModel
{
    public class NewPasswordModel : EmailModelBase
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }
}