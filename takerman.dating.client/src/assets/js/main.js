function toggleNavbarMethod() {
    if (window.width > 992) {
        document.getElementsByClassName('.navbar .dropdown').on('mouseover', function () {
            document.getElementsByClassName('.dropdown-toggle', this).click();
        }).on('mouseout', function () {
            document.getElementsByClassName('.dropdown-toggle', this).click().blur();
        });
    } else {
        document.getElementsByClassName('.navbar .dropdown').off('mouseover').off('mouseout');
    }
}

toggleNavbarMethod();

window.onresize = toggleNavbarMethod;

window.onscroll = function () {
    if (this.scrollTop() > 100) {
        document.getElementsByClassName('.back-to-top').fadeIn('slow');
    } else {
        document.getElementsByClassName('.back-to-top').fadeOut('slow');
    }
};

document.getElementsByClassName('.back-to-top').onclick = function () {
    $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
    return false;
};

document.getElementsByClassName('.vendor-carousel').owlCarousel({
    loop: true,
    margin: 29,
    nav: false,
    autoplay: true,
    smartSpeed: 1000,
    responsive: {
        0: {
            items: 2
        },
        576: {
            items: 3
        },
        768: {
            items: 4
        },
        992: {
            items: 5
        },
        1200: {
            items: 6
        }
    }
});

document.getElementsByClassName('.related-carousel').owlCarousel({
    loop: true,
    margin: 29,
    nav: false,
    autoplay: true,
    smartSpeed: 1000,
    responsive: {
        0: {
            items: 1
        },
        576: {
            items: 2
        },
        768: {
            items: 3
        },
        992: {
            items: 4
        }
    }
});


document.getElementsByClassName('.quantity button').onclick = function () {
    var button = $(this);
    var oldValue = button.parent().parent().find('input').val();
    if (button.hasClass('btn-plus')) {
        var newVal = parseFloat(oldValue) + 1;
    } else {
        if (oldValue > 0) {
            var newVal = parseFloat(oldValue) - 1;
        } else {
            newVal = 0;
        }
    }
    button.parent().parent().find('input').val(newVal);
}