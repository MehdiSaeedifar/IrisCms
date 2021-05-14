using Iris.Utilities.Mail;

namespace Iris.Web.Email
{
    public interface IEmailService
    {
        SendingMailResult Send(MailDocument doc);

        SendingMailResult SendCommentReplyNotification(CommentReplyEmailNotificationData data, CommentReplyType replyType);

        SendingMailResult SendResetPasswordConfirmationEmail(string userName, string email, string key);

        SendingMailResult SendNewPassword(string userName, string email, string newPassword);
    }
}
