namespace Iris.Model.EmailModel
{
    public class ResetPasswordModel : EmailModelBase
    {
        public string UserName { get; set; }
        public string ResetLink { get; set; }
    }
}