<template>
    <section id="breadcrumbs" class="breadcrumbs">
        <div class="container">
            <ol>
                <li><router-link to="/">Home</router-link></li>
                <li><router-link to="/uploads">Uploads</router-link></li>
                <li>Print</li>
            </ol>
        </div>
    </section>
    <section class="container">
        <h2 class="text-center">Print</h2><br />
        <div id="accordion">
            <div class="card">
                <div @click="collapse('configure')" class="card-header" id="headingConfigure">
                    <h5 class="mb-0">
                        <button class="btn btn-link" data-toggle="collapse" data-target="#collapseConfigure"
                            aria-expanded="true" aria-controls="collapseConfigure">
                            Specifications
                        </button>
                    </h5>
                </div>

                <div id="collapseConfigure" class="collapse show" aria-labelledby="headingConfigure"
                    data-parent="#accordion">
                    <div class="card-body">
                        <hgroup>
                            <h3>Specifications</h3>
                        </hgroup>
                        <br />
                        <div class="form-group row">
                            <p class="text-center">
                                The maximum size of the model should be 220mm width, 220mm length, 250mm height. <br />
                                If is not we would resize it.<br />
                                <img id="specificationMaxSize" src="/src/assets/img/3d-max-size.jpg" class="img-fluid"
                                    alt="max size of print">
                            </p>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <strong id="printName" class="col-form-label">{{ this.print?.name }}</strong>
                            </div>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <span id="printColorLabel" class="col-form-label">Color</span>
                                    </div>
                                    <div class="col-sm-8">
                                        <select placeholder="Select color" required id="printColor" class="form-control"
                                            v-model="this.fields.selectedColor" aria-describedby="helpId">
                                            <option v-for="(color, key) in colors" :key="key" :value="color.id">{{
                                                color.name }}
                                            </option>
                                        </select>
                                        <small id="helpId" class="form-text text-muted">How many printed copies of the item
                                            would you want</small>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <span id="printQuantityLabel" class="col-form-label">Quantity</span>
                                    </div>
                                    <div class="col-sm-8">
                                        <input type="number" class="form-control" name="printQuantity"
                                            aria-describedby="helpId" placeholder="Quantity" :value="this.payment.quantity">
                                        <small id="helpId" class="form-text text-muted">How many printed copies of the item
                                            would you want</small>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <button @click="confirmSection('configure')"
                                    class="btn btn-success text-center">Confirm</button>
                            </div>
                            <div class="col-sm-10">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card">
                <div @click="collapse('shipping')" class="card-header" id="headingShipping">
                    <h5 class="mb-0">
                        <button class="btn btn-link collapsed" data-toggle="collapse" data-target="#collapseShipping"
                            aria-expanded="false" aria-controls="collapseShipping">
                            Shipping
                        </button>
                    </h5>
                </div>
                <div id="collapseShipping" class="collapse" aria-labelledby="headingShipping" data-parent="#accordion">
                    <div class="card-body">
                        <h3>Shipping</h3>
                        <div class="form-group row">
                            <label for="firstName" class="col-sm-2 col-form-label">First Name</label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="firstName" placeholder="First Name"
                                    v-model="fields.firstName" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="lastName" class="col-sm-2 col-form-label">Last Name</label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="lastName" placeholder="Last Name"
                                    v-model="fields.lastName" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="country" class="col-sm-2 col-form-label">Country</label>
                            <div class="col-sm-10">
                                <input type="text" required class="form-control" id="country" placeholder="Country"
                                    v-model="fields.country" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="city" class="col-sm-2 col-form-label">City</label>
                            <div class="col-sm-10">
                                <input type="text" required class="form-control" id="city" placeholder="City"
                                    v-model="fields.city" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="street" class="col-sm-2 col-form-label">Street</label>
                            <div class="col-sm-10">
                                <input type="text" required class="form-control" id="street" placeholder="Street"
                                    v-model="fields.street" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="number" class="col-sm-2 col-form-label">Street Number</label>
                            <div class="col-sm-10">
                                <input type="number" required class="form-control" id="number" placeholder="Street Number"
                                    v-model="fields.number" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="phone" class="col-sm-2 col-form-label">Phone</label>
                            <div class="col-sm-10">
                                <input type="phone" required class="form-control" id="phone" placeholder="Phone"
                                    v-model="fields.phone" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <button type="button" @click="confirmSection('shipping')"
                                    class="btn btn-success text-center">Confirm</button>
                            </div>
                            <div class="col-sm-10">
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
            <div class="card">
                <div @click="collapse('payment')" class="card-header" id="headingPayment">
                    <h5 class="mb-0">
                        <button class="btn btn-link collapsed" data-toggle="collapse" data-target="#collapsePayment"
                            aria-expanded="false" aria-controls="collapsePayment">
                            Payment
                        </button>
                    </h5>
                </div>
                <div id="collapsePayment" class="collapse" aria-labelledby="headingPayment" data-parent="#accordion">
                    <div class="card-body">
                        <h3>Payment</h3>

                        <div class="form-group row">
                            <div class="col-sm-8">
                                <p>
                                    We operate with flat rate + shipping paid from you. <br />
                                    A model cost {{ this.payment.price }} EUR. <br />
                                    Shipping cost is {{ this.payment.shippingPrice }} EUR.
                                </p>
                            </div>
                            <div class="col-sm-4">
                                <h3>Total: {{ this.total }} EUR</h3>
                                <div v-if="!this.payment.paid" id="paypal-button-container"></div>
                                <div v-else id="confirmation">Order complete!</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div v-show="status"
            :class="{ 'alert-success': this.statusClass == 'success', 'alert-danger': this.statusClass == 'danger' }"
            class="alert" role="alert">
            <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
            <span class="sr-only">{{ status }}</span>
        </div>
    </section>
</template>

<script lang="js">
import { loadScript } from '@paypal/paypal-js'
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';

export default {
    data() {
        return {
            fields: {
                id: 0,
                userId: 0,
                selectedColor: 1,
                firstName: '',
                lastName: '',
                country: '',
                city: '',
                street: '',
                number: 0,
                phone: ''
            },
            payment: {
                quantity: 1,
                price: 10.00,
                shippingPrice: 2.50,
                paid: false,
                currency: "EUR"
            },
            id: 0,
            colors: null,
            print: null,
            status: '',
            statusClass: ''
        };
    },
    async beforeCreate() {
        try {
            let queryString = window.location.search;
            let urlParams = new URLSearchParams(queryString);

            if (urlParams.has('id')) {
                this.id = urlParams.get('id');
                this.print = await fetchWrapper.get('/Upload/Get?id=' + this.id);
            }

            this.colors = await fetchWrapper.get('/Upload/GetColors');

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
    async mounted() {
        const authStore = useAuthStore();

        this.fields.firstName = authStore.user.firstName;
        this.fields.lastName = authStore.user.lastName;

        const address = await fetchWrapper.get('Address/GetByUserId?id=' + authStore.user.id);
        if (address.id && address.id != 0) {
            this.fields.id = address.id;
            this.fields.country = address.country;
            this.fields.city = address.city;
            this.fields.street = address.street;
            this.fields.number = address.number;
            this.fields.phone = address.phone;
            this.fields.userId = address.userId;
        }

        this.confirmSection('payment');
    },
    computed: {
        total() {
            return this.payment.quantity * this.payment.price + this.payment.shippingPrice;
        },
        guid() {
            return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
                (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
            );
        }
    },
    methods: {
        collapse(section) {
            let collapseConfigure = document.getElementById('collapseConfigure');
            let collapseShipping = document.getElementById('collapseShipping');
            let collapsePayment = document.getElementById('collapsePayment');

            switch (section) {
                case 'configure':
                    collapseConfigure.classList.toggle('show');
                    break;

                case 'shipping':
                    collapseShipping.classList.toggle('show');
                    break;

                case 'payment':
                    collapsePayment.classList.toggle('show');
                    break;

                default:
                    break;
            }
        },
        confirmSection(section) {
            let collapseConfigure = document.getElementById('collapseConfigure');
            let collapseShipping = document.getElementById('collapseShipping');
            let collapsePayment = document.getElementById('collapsePayment');

            switch (section) {
                case 'configure':
                    collapseConfigure.classList.remove('show');
                    collapseShipping.classList.add('show');
                    collapsePayment.classList.remove('show');
                    break;

                case 'shipping':
                    collapseConfigure.classList.remove('show');
                    collapseShipping.classList.remove('show');
                    collapsePayment.classList.add('show');
                    break;

                case 'payment':
                    collapseConfigure.classList.add('show');
                    collapseShipping.classList.remove('show');
                    collapsePayment.classList.remove('show');
                    break;

                default:
                    break;
            }
        },
        async createOrder(data, actions) {
            return actions.order.create({
                purchase_units: [
                    {
                        amount: {
                            currency_code: this.payment.currency,
                            value: this.total
                        },
                        reference_id: this.guid
                    }
                ],
                intent: "CAPTURE"
            });
        },
        onApprove(data, actions) {
            return actions.order.capture().then(async () => {
                this.payment.paid = true;
                const authStore = useAuthStore();
                let data = {
                    total: this.total,
                    firstName: this.fields.firstName,
                    lastName: this.fields.lastName,
                    currency: this.payment.currency,
                    street: this.fields.street,
                    city: this.fields.city,
                    country: this.fields.country,
                    number: this.fields.number,
                    phone: this.fields.phone,
                    quantity: this.payment.quantity,
                    paymentProvider: 'PayPal',
                    userId: authStore.user.id,
                    colorId: this.fields.selectedColor,
                    uploadId: this.print.id,
                    trackingCode: ''
                };
                let response = await fetchWrapper.post('Order/Create', data);
            });
        }
    }
}
</script>

<style scoped>
.btn-link:hover,
.btn-link,
.btn-link:focus {
    text-decoration: none;
}

#specificationMaxSize {
    width: 200px;
    height: 200px;
}

#paypal-button-container {
    width: 100%;
    margin: 30px 0;
}

#confirmation {
    color: green;
    margin-top: 1em;
    font-size: 2em;
}
</style>