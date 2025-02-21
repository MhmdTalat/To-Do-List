<script>


    // Handle quantity increment and decrement
    $(document).ready(function () {
        $('.btn-number').click(function (e) {
            e.preventDefault();
            var fieldName = $(this).attr('data-type');
            var quantityInput = $(this).closest('.product-qty').find('input[name="quantity"]');
            var currentVal = parseInt(quantityInput.val());

            if (!isNaN(currentVal)) {
                // Decrease quantity by 1, but not below 1
                if (fieldName === 'minus' && currentVal > 1) {
                    quantityInput.val(currentVal);
                }
                // Increase quantity by 1, but not above the max (10)
                else if (fieldName === 'plus' && currentVal < 10) {
                    quantityInput.val(currentVal + 1);
                }
            } else {
                // Set to 1 if the input is not a valid number
                quantityInput.val(1);
            }
        });
    });

</script>