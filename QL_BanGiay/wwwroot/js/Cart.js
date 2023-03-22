var cartId = "cart";

var localAdapter = {

    saveCart: function (object) {

        var stringified = JSON.stringify(object);
        localStorage.setItem(cartId, stringified);
        return true;

    },
    getCart: function () {

        return JSON.parse(localStorage.getItem(cartId));

    },
    clearCart: function () {

        localStorage.removeItem(cartId);
    },
    removeCart: function (id, size) {
        var Cart = storage.getCart();
        var newCart = [];
        $.each(Cart, function (index, item) {
            if ((item.id != id) || (item.id == id && item.idsize != size)) {
                newCart.push(item);
            }
        });
        localAdapter.saveCart(newCart);
        $(".row_" + id + "-" + size).remove();
        document.getElementById("cartCount").textContent = storage.getCart().length;
        var item = storage.getCart();
        cart.setItems(item);
        helpers.updateViewCart();
    }

};

var ajaxAdapter = {

    saveCart: function (object) {

        var stringified = JSON.stringify(object);
        // do an ajax request here

    },
    getCart: function () {

        // do an ajax request -- recognize user by cookie / ip / session
        return JSON.parse(data);

    },
    clearCart: function () {

        //do an ajax request here

    }

};

var storage = localAdapter;

var helpers = {

    getHtml: function (id) {

        return document.getElementById(id).innerHTML;

    },
    setHtml: function (id, html) {

        document.getElementById(id).innerHTML = html;
        return true;

    },
    itemData: function (object) {

        var count = object.querySelector(".count"),
            patt = new RegExp("^[1-9]([0-9]+)?$");
        count.value = (patt.test(count.value) === true) ? parseInt(count.value) : 1;
        var select = document.getElementById('size');
        var selectText = select.options[select.selectedIndex].text;
        var selectValue = select.options[select.selectedIndex].value;
        var item = {

            name: object.getAttribute('data-name'),
            price: object.getAttribute('data-price'),
            id: object.getAttribute('data-id'),
            image: object.getAttribute('data-image'),
            size: selectText,
            idsize: selectValue,
            count: count.value,
            total: parseInt(object.getAttribute('data-price')) * parseInt(count.value)

        };
        return item;
    },
    itemCart: function (object) {
        var counts = object.querySelector(".count");
        var item = {
            id: object.getAttribute('data-id'),
            size: object.getAttribute('data-size'),
            count: counts.value,
            total: parseInt(object.getAttribute('data-price')) * parseInt(counts.value)
        };
        return item;
    },
    updateView: function () {

        var items = cart.getItems(),
            template = this.getHtml('cartT'),
            compiled = _.template(template, {
                items: items
            });
        this.setHtml('cartItems', compiled);
        this.updateTotal();
    },
    updateViewCart: function () {

        var items = cart.getItems(),
            template = this.getHtml('cartT'),
            compiled = _.template(template, {
                items: items
            });
        this.setHtml('cartItems', compiled);
        this.updateTotalCart();
    },
    emptyView: function () {

        this.setHtml('cartItems', '<p>Giỏ hàng đang trống</p>');
        this.updateTotal();

    },
    updateTotal: function () {
        var id = document.getElementById('totalPrice');
        this.setHtml('totalPrice', cart.total + ' VND');
    },
    updateTotalCart: function () {
        var id = document.getElementById('totalPrice');
        var items = storage.getCart();
        var total = 0;
        for (var i = 0; i < items.length; i++) {
            var _item = items[i];
            total += _item.total;
        }
        this.setHtml('totalPrice', total + ' VND');
    }

};

var cart = {

    count: 0,
    total: 0,
    items: [],
    getItems: function () {

        return this.items;

    },
    setItems: function (items) {

        this.items = items;
        for (var i = 0; i < this.items.length; i++) {
            var _item = this.items[i];
            this.total += _item.total;
        }

    },
    clearItems: function () {

        this.items = [];
        this.total = 0;
        storage.clearCart();
        helpers.emptyView();

    },
    addItem: function (item) {

        if (this.containsItem(item.id, item.size) === false) {

            this.items.push({
                id: item.id,
                name: item.name,
                size: item.size,
                idsize: item.idsize,
                price: item.price,
                image: item.image,
                count: item.count,
                total: item.price * item.count
            });

            storage.saveCart(this.items);


        } else {

            this.updateItem(item);

        }
        this.total += item.price * item.count;
        this.count += item.count;
        helpers.updateView();

    },
    containsItem: function (id, size) {

        if (this.items === undefined) {
            return false;
        }

        for (var i = 0; i < this.items.length; i++) {

            var _item = this.items[i];
            if (id == _item.id && size == _item.size) {
                return true;
            }

        }
        return false;

    },
    updateItem: function (object) {

        for (var i = 0; i < this.items.length; i++) {

            var _item = this.items[i];

            if (object.id === _item.id && object.size === _item.size) {

                _item.count = parseInt(object.count) + parseInt(_item.count);
                _item.total = parseInt(object.total) + parseInt(_item.total);
                this.items[i] = _item;
                storage.saveCart(this.items);

            }
        }
    },
    updateItemCart: function (object) {
        for (var i = 0; i < this.items.length; i++) {

            var _item = this.items[i];

            if (object.id === _item.id && object.size === _item.size) {
                _item.count = parseInt(object.count);
                _item.total = parseInt(object.total);
                this.items[i] = _item;
                storage.saveCart(this.items);
            }
        }
    }
};
document.addEventListener('DOMContentLoaded', function () {
    if (storage.getCart()) {
        cart.setItems(storage.getCart());
        helpers.updateView();
        document.getElementById("cartCount").textContent = storage.getCart().length;
    } else {
        helpers.emptyView();
        document.getElementById("cartCount").textContent = 0;
    }
});
$('.minus-btn,.minus-btn-1').on('click', function (e) {
    e.preventDefault();
    var $this = $(this);
    var $input = $this.closest('div').find('input');
    var value = parseInt($input.val());

    if (value > 1) {
        value = value - 1;
    } else {
        value = 1;
    }
    $input.val(value);
});

$('.plus-btn,.plus-btn-1').on('click', function (e) {
    e.preventDefault();
    var $this = $(this);
    var $input = $this.closest('div').find('input');
    var value = parseInt($input.val());

    if (value < 100) {
        value = value + 1;
    } else {
        value = 100;
    }
    $input.val(value);
});