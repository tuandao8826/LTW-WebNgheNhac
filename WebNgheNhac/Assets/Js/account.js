/* account */
const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

const accNavItemLinks = $$('.nav__list-item a');

const account = {
    handleEvent: function () {
        // xử lý nút active trong thanh navbar bên trái
        window.onload = function () {
            var path = window.location.pathname;
            accNavItemLinks.forEach(function (item) {
            if (item.pathname == path) {
                var accNavItem = item.parentNode;
                $('.nav__list-item.active').classList.remove('active');
                accNavItem.classList.add('active');
            }
            })
        }
    },

    start: function () {
        this.handleEvent();
    }
}

account.start();