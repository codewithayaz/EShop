$(function () {

    $(".addToCart").click(function () {
        event.preventDefault();
        let id = this.dataset.id;
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            type: 'POST',
            url: '/api/cart/additem',
            data: id,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (result) {
                toastr.success(result)
            },
            error: function (error) {
                if (error.status == 404) {
                    toastr.error(error.responseJSON);
                }
                else if (error.status == 401) {
                    Swal.fire({
                        title: 'Re-Login to start shopping.',
                        text: "",
                        icon: 'info',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, proceed to login'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = "/Account/Login";
                        }
                    });
                }
                else if (error.status == 403) {
                    toastr.error('You do not have access to this resource.');
                }
                else {

                }
            }
        });
    });

    $(".btnRemoveFromCart").click(function () {
        event.preventDefault();
        let id = this.dataset.id;
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            type: 'POST',
            url: '/api/cart/removeitem',
            data: id,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (result) {
                toastr.info(result);
                window.location.reload();
            },
            error: function (error) {
                
            }
        });
    });

});

