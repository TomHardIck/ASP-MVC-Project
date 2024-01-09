$(
    function () {
        var placeholderElement = $('#PlaceHolderHere');
        var button = $('button[data-toggle="ajax-modal"]');
        button.click(function (event) {
            var url = $(this).data('url');
            var decodedUrl = decodeURIComponent(url);
            $.get(decodedUrl).done(function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            })
        })
    }
)