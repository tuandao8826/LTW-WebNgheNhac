/* admin */
const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

const admNavItemLinks = $$('.nav__list-item a');

const admin = {
    handleEvent: function () {
        // xử lý nút active trong thanh navbar bên trái
        window.onload = function () {
            var path = window.location.pathname;
            admNavItemLinks.forEach(function (item) {
                if (item.pathname == path) {
                    var admNavItem = item.parentNode;
                    $('.nav__list-item.active').classList.remove('active');
                    admNavItem.classList.add('active');
                }
            })
        }
    },

    start: function () {
        this.handleEvent();
    }
}

admin.start();