/// <reference path="jquery-1.9.1.intellisense.js" />
/// <reference path="jquery-1.9.1.js" />

var Message = new Object();
Message.success = '<div id="alert" class="alert alert-success"><button type="button" class="close" data-dismiss="alert">×</button>رای شما با موفقیت ثبت شد</div>';
Message.duplicate = '<div id="alert" class="alert alert-info"><button type="button" class="close" data-dismiss="alert">×</button>قبلا رای داده اید!</div>';
Message.error = '<div id="alert" class="alert alert-warning"><button type="button" class="close" data-dismiss="alert">×</button>خطایی صورت گرفته است</div>';
Message.forbidden = '<div id="alert" class="alert alert-warning"><button type="button" class="close" data-dismiss="alert">×</button>برای رای دادن باید عضو سایت باشید</div>';

var LogOnForm = new Object();

LogOnForm.onSuccess = function () {
    $('#logOnModal').modal('show');
};



(function ($) {

    $('#myCarousel').oneCarousel({
        interval: 5000,
        pause: 'hover',
    });

    $('[data-i-search-input=true]').on('focus', function () {
        $(this).stop().animate({ 'width': '+=60' }, 'slow');
    });

    $('[data-i-search-input=true]').on('blur', function () {
        $(this).stop().animate({ 'width': '-=60' }, 'slow');
    });

    $('button.btn-advanced-search').on('click', function () {
        $('div#search-advanced').slideToggle();
    });

    $('div#header-social-networks-container img,div#post-book-share img').hover(function () {
        $(this).addClass('animate0 rotateIn');
    }, function () {
        $(this).removeClass('animate0 rotateIn');
    });

    $('#btn-reply').on('click', function () {
        $('#comment-reply1').slideToggle('slow');
    });
})(jQuery);

var Book = new Object();
Book.onSuccess = function () { };
Book.onBegin = function () {
};

var LikePost = new Object();
LikePost.onBegin = function () { };

$('#post-like,#post-dis-like').on('click', function (event) {
    event.preventDefault();
    var $this = $(this);
    var $link = $this.children('span');
    var resultDiv = $('div#like-result');
    resultDiv.html('<img src="' + resultDiv.attr('data-loading-image') + '" />').fadeIn('slow');

    $.ajax({
        type: "POST",
        url: $link.attr('data-href'),
        dataType: "json",
        success: function (response) {
            resultDiv.fadeOut();
            switch (response.result) {
                case "success":
                    resultDiv.html(Message.success);
                    $('#like-count').html(response.count);
                    break;
                case "duplicate":
                    resultDiv.html(Message.duplicate);
                    break;
            }
            resultDiv.fadeIn('slow');
        },
        error: function (xhr) {
            resultDiv.html(Message.error).fadeIn('slow');
        },
        complete: function (xhr, status) {
            var data = xhr.responseText;
            if (xhr.status == 403 || xhr.status == 401) {
                resultDiv.html(Message.forbidden).fadeIn('slow');
            }
        }
    });
});


var AddComment = new Object();

AddComment.onSuccess = function (data) {
    if (data.show == "false") {
        var message = '<div id="alert" class="alert alert-success"><button type="button" class="close" data-dismiss="alert">×</button>دیدگاه شما ثبت شد و پس از تایید،نمایش داده خواهد شد</div>';
        $('#addCommentResult').html(message);
    } else if (data.show == "true") {

    }
    else {
        $('#commentReply').html(data);
        validateForm('frmCommentReply');
    }

};

AddComment.slideToggle = function () {
    $('.comment-reply-container').slideUp().html('');
    $('#addComment').slideDown('slow');
    validateForm('frmCommentReply');
};

var CommentReply = new Object();

CommentReply.slideDown = function (id, data, third) {

    $('#comment-reply-' + id).slideDown('slow');
    validateForm('frmCommentReply');
};
CommentReply.onBegin = function () {
    $('.comment-reply-container').slideUp().html('');
    $('#addComment').slideUp().html('');
};


var CommentLike = new Object();

$('div.comment-like-link').children('span').on('click', function (event) {
    event.preventDefault();
});

$('div.comment-like-link').on('click', function () {
    var $this = $(this).children('span:first');
    var $loading = $('div#commentLikeLoading' + $this.attr('data-comment-id')).fadeIn();
    $.ajax({
        type: "POST",
        url: $this.attr('data-href'),
        success: function (response) {
            CommentLike.onSuccess(response);
        },
        statusCode: {
            403: function () {
                $('#comment-like-result-' + $this.attr('data-comment-id')).html(Message.forbidden).fadeIn('slow');
            },
            401: function () {
                $('#comment-like-result-' + $this.attr('data-comment-id')).html(Message.forbidden).fadeIn('slow');
            }
        },
        error: function () {
            CommentLike.onFailure();
        }
    });
    $loading.fadeOut();
});

CommentLike.onSuccess = function (data) {

    if (data.result == "success") {
        $('#comment-like-count-' + data.commentId).text(data.count);
        $('#comment-like-result-' + data.commentId).html(Message.success).fadeIn('slow');
    } else if (data.result == "duplicate") {
        $('#comment-like-result-' + data.commentId).html(Message.duplicate).fadeIn('slow');
    }

};


CommentLike.onFailure = function () {
    $('#comment-like-result-' + CommentLike.commentId).html(Message.error).fadeIn('slow');
};



var Comment = new Object();

Comment.ChangePage = new Object();

var RegisterUser = new Object();
RegisterUser.Form = new Object();
RegisterUser.Form.onSuccess = function (data) {
    if (data.result == "success") {
        var message = '<div id="alert" class="alert alert-success"><button type="button" class="close" data-dismiss="alert">×</button>اطلاعات شما با موفقیت در سیستم ثبت شد.چند لحظه منتظر بمانید...</div>';
        $('#registerResult').html(message);
        setTimeout(RegisterUser.redirect, 3000);
    }
    else {
        $('#logOnModal').html(data);
    }
};

RegisterUser.redirect = function () {
    window.location.reload(true);
};

RegisterUser.Form.validateEmail = function (url) {
    var $email = $('#registerEmail');
    var $loading = $email.siblings('img:first');
    $email.on('change', function (event) {
        $loading.fadeIn('slow');
        $.post(url, { email: $email.val() }, function (data) {
            $loading.fadeOut('fast');
            if (data.isExist === true) {
                $('#emailValidationDiv').html('ایمیل تکراری!').css('color', 'red');
            } else {
                $('#emailValidationDiv').html('آزاد').css('color', 'green');
            }
        });
    });
};

RegisterUser.Form.validateUserName = function (url) {
    var $userName = $('#UserName');
    var $loading = $userName.siblings('img:first');
    $userName.on('change', function (event) {
        $loading.fadeIn('slow');
        $.post(url, { userName: $userName.val() }, function (data) {
            $loading.fadeOut('fast');
            if (data.isExist === true) {
                $('#userNameValidationDiv').html('نام کاربری تکراری!').css('color', 'red');
            } else {
                $('#userNameValidationDiv').html('آزاد').css('color', 'green');
            }
        });
    });
};

RegisterUser.ForgottenPassword = new Object();

RegisterUser.ForgottenPassword.Form = new Object();

RegisterUser.ForgottenPassword.Form.onSuccess = function (data) {
    if (data.result === "true") {

        $('#resetResult').html('<div id="alert" class="alert alert-success"><button type="button" class="close" data-dismiss="alert">×</button>' + data.message + '</div>');
    } else if (data.result === "false") {
        $('#resetResult').html('<div id="alert" class="alert alert-warning"><button type="button" class="close" data-dismiss="alert">×</button>' + data.message + '</div>');
    } else {
        $('#logOnModal').html(data);
    }
};

$(document).ready(function () {
    autocompleteBinding = function () {
        $('input[data-autocomplete=true]').each(function () {
            $(this).autocomplete({
                source: $(this).attr('data-autocomplete-url'),
                minLength: 2,
                delay: 5,
                select: function (event, ui) {
                    window.location = ui.item.url;
                },
                position: { my: "right top", at: "right bottom", collision: "none" }
            });

        });
    };
    autocompleteBinding();
});

function validateForm(id) {
    $.validator.unobtrusive.parse('#' + id);
}

function reloadPage() {
    window.location.reload();
}

var ContactUs = new Object();

ContactUs.Form = new Object();

ContactUs.Form.onSuccess = function (data) {
    if (data.result === "success") {

    } else {
        $('#main-content').html(data);
    }
};

(function () {
    var currentPath = window.location.pathname;
    $('div#sidebar li a').each(function () {
        var $this = $(this);
        if ($this.attr('href') === currentPath) {
            $this.parent('li').addClass('active');
        }
    });
    $('div.navbar ul.nav li > a').each(function () {
        var $this = $(this);
        if ($this.attr('href') === currentPath) {
            $this.parent('li').addClass('active');
        }
    });

    $('[data-slide]').on('click', function (e) {
        e.preventDefault();
    });

})();

(function ($) {
    $.fn.InfiniteScroll = function (options) {
        var defaults = {
            moreInfoDiv: '#MoreInfoDiv',
            progressDiv: '#Progress',
            loadInfoUrl: '/',
            loginUrl: '/login',
            errorHandler: null,
            completeHandler: null,
            noMoreInfoHandler: null
        };
        options = $.extend(defaults, options);

        var showProgress = function () {
            $(options.progressDiv).css("display", "block");
        };

        var hideProgress = function () {
            $(options.progressDiv).css("display", "none");
        };

        return this.each(function () {
            var moreInfoButton = $(this);
            var page = 1;
            $(moreInfoButton).click(function (event) {
                event.preventDefault();
                showProgress();
                $.ajax({
                    type: "POST",
                    url: options.loadInfoUrl,
                    data: JSON.stringify({ page: page }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function (xhr, status) {
                        var data = xhr.responseText;
                        if (xhr.status == 403) {
                            window.location = options.loginUrl;
                        }
                        else if (status === 'error' || !data) {
                            if (options.errorHandler) {
                                hideProgress();
                                options.errorHandler(this);
                            }
                        }
                        else {
                            if (data === 'no-more') {
                                if (options.noMoreInfoHandler) {
                                    hideProgress();
                                    options.noMoreInfoHandler(this);
                                }
                            }
                            else {
                                hideProgress();
                                var $boxes = $(data);
                                $(options.moreInfoDiv).append($boxes);
                                console.log(++page);
                            }

                        }
                        hideProgress();
                        if (options.completeHandler)
                            options.completeHandler(this);
                    }
                });
            });
        });
    };
})(jQuery);

$("#moreInfoButton").InfiniteScroll({
    moreInfoDiv: '#MoreInfoDiv',
    progressDiv: '#loadingMessage',
    loadInfoUrl: $("#moreInfoButton").attr('href'),
    errorHandler: function () {
        var noty = window.noty({ text: "خطایی رخ داده است", type: 'error', timeout: 2500 });
    },
    noMoreInfoHandler: function () {
        var noty = window.noty({ text: "مطلب بیشتری در دسترس نیست", type: 'information', timeout: 2500 });
    }
});