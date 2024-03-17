function toggleNavbarMethod() {
    let dropdowns = document.querySelectorAll('.navbar .dropdown');
    for (let i = 0; i < dropdowns.length; i++) {
        const dropdown = dropdowns[i];
        if (window.width > 992) {
            dropdown.onmouseover = function () {
                document.querySelector('.dropdown-toggle').click();
            }
            dropdown.onmouseout = function () {
                document.querySelector('.dropdown-toggle').click().blur();
            }
        } else {
            dropdown.removeEventListener('mouseover', function(){});
            dropdown.removeEventListener('mouseout', function(){});
        }
    }
}

toggleNavbarMethod();

window.onresize = toggleNavbarMethod;

// let lastKnownScrollPosition = 0;
// let ticking = false;

// document.addEventListener("scroll", (event) => {
//     lastKnownScrollPosition = window.scrollY;

//     if (!ticking) {
//         window.requestAnimationFrame(() => {
//             ticking = false;
//             document.querySelector('.back-to-top').style.display = 'block';
//         });

//         ticking = true;
//         document.querySelector('.back-to-top').style.display = 'none';
//     }
// });

// document.querySelector('.back-to-top').onclick = function () {
//     document.querySelectorAll('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
//     return false;
// };