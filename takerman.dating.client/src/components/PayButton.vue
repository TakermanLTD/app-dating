<template>
    <div v-if="this.dateId" id="paypal-button-container"></div>
</template>

<script lang="js">
import { loadScript } from '@paypal/paypal-js';
import { useAuth0 } from '@auth0/auth0-vue';

export default {
    props: {
        dateId: Number,
        onApprove: Function
    },
    data() {
        return {
            date: null,
            payment: {
                price: 0,
                currency: "EUR"
            }
        }
    },
    async beforeCreate() {
        try {
            if (this.dateId) {
                this.date = await fetch('Dates/Get?id=' + this.dateId);
            }

            loadScript({ 'client-id': 'AQeperGzY3bUm7hiKg8OcfKHzMVxg6ItAMHuMUSvoc3ZWwP6OYSsUvzxnSqtP0ryDcIffn6MT_h9klly', currency: this.payment.currency })
                .then((paypal) => {
                    paypal.Buttons({
                        createOrder: this.createOrder,
                        onApprove: this.onApprove,
                    }).render('#paypal-button-container')
                })
        }
        catch (ex) {
            console.log(ex);
        }
    },
    computed: {
        guid() {
            return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
                (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
            );
        }
    },
    methods: {
        async createOrder(data, actions) {
            return actions.order.create({
                purchase_units: [
                    {
                        amount: {
                            currency_code: this.payment.currency,
                            value: this.date.price
                        },
                        reference_id: this.guid
                    }
                ],
                intent: "CAPTURE"
            });
        },
        onApprove(data, actions) {
            return actions.order.capture().then(async () => {
                const { user } = useAuth0();
                let data = {
                    total: this.date.price,
                    currency: this.payment.currency,
                    userId: user?.id,
                    dateId: this.date.id
                };
                let response = await fetch('Order/Create', data);
            });
        }
    },
}
</script>