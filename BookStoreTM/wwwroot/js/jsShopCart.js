﻿$(document).ready(function () {
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('.cart-plus-minus-box').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        alert(id + " " + quatity);
    })
})