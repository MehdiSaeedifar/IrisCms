namespace Iris.Servicelayer.EFServices.Enums
{
    public enum VerifyUserStatus
    {
        VerifiedSuccessfully,
        UserIsbaned,
        VerifiedFaild
    }

    public enum AddUserStatus
    {
        UserNameExist,
        EmailExist,
        AddingUserSuccessfully,
    }

    public enum EditedUserStatus
    {
        UserNameExist,
        EmailExist,
        UserNameOrEmailExist,
        UpdatingUserSuccessfully
    }

    public enum ChangePasswordResult
    {
        ChangedSuccessfully,
        ChangedFaild
    }

    public enum Order
    {
        Asscending,
        Descending
    }

    public enum PostOrderBy
    {
        ById,
        ByTitle,
        ByPostAuthor,
        ByVisitedNumber,
        ByLabels
    }

    public enum UserOrderBy
    {
        UserName,
        PostCount,
        CommentCount,
        RegisterDate,
        IsBaned,
        LoginDate,
        Ip
    }

    public enum UserSearchBy
    {
        UserName,
        FirstName,
        LastName,
        Email,
        Ip,
        RoleDescription
    }

    public enum LabelSearchBy
    {
        Name,
        Description
    }

    public enum LabelOrderBy
    {
        Id,
        Name,
        Description,
        PostCount
    }

    public enum UpdatePostStatus
    {
        Successfull,
        Faild
    }

    public enum CommentOrderBy
    {
        AddDate,
        IsApproved,
        LikeCount,
        UserName,
        Author,
        Ip
    }

    public enum CommentSearchBy
    {
        UserName,
        Author,
        Body,
        Ip
    }

    public enum PageSearchBy
    {
        Title,
        UserName,
        ParentTitle
    }

    public enum PageOrderBy
    {
        Title,
        Date,
        CommentCount,
        Status,
        UserName,
        ParentTitle
    }

    public enum CategorySearchBy
    {
        Name,
        Description
    }

    public enum CategoryOrderBy
    {
        Id,
        Name
    }

    public enum ArticleSearchBy
    {
        Title,
        UserName,
    }

    public enum ArticleOrderBy
    {
        Title,
        Date,
        CommentCount,
        Status,
        UserName,
        Order
    }
}