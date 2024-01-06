<template>
    <fieldset class="container">
        <hgroup>
            <h4>Address</h4>
        </hgroup>
        <form @submit="save">
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
                    <input type="text" required class="form-control" id="city" placeholder="City" v-model="fields.city" />
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
                <label for="number" class="col-sm-2 col-form-label">Number</label>
                <div class="col-sm-10">
                    <input type="number" required class="form-control" id="number" placeholder="Number"
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
                    <button id="btnSubmit" class="btn btn-success text-center">Save</button>
                </div>
                <div class="col-sm-10">
                    <div v-if="this.loading">Loading...</div>
                    <div v-else v-show="status"
                        :class="{ 'alert-success': this.statusClass == 'success', 'alert-danger': this.statusClass == 'danger' }"
                        class="alert" role="alert">
                        <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                        <span class="sr-only">{{ status }}</span> 
                    </div>
                </div>
            </div>
            <br />
        </form>
    </fieldset>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';

export default {
    data() {
        return {
            fields: {
                id: 0,
                userId: 0,
                country: '',
                city: '',
                street: '',
                number: 0,
                phone: ''
            },
            loading: false,
            status: '',
            statusClass: ''
        };
    },
    async mounted() {
        const authStore = useAuthStore();
        this.loading = true;
        const address = await fetchWrapper.get('Address/GetByUserId?id=' + authStore.user.id);
        this.loading = false;
        if (address.id && address.id != 0) {
            this.fields.id = address.id;
            this.fields.country = address.country;
            this.fields.city = address.city;
            this.fields.street = address.street;
            this.fields.number = address.number;
            this.fields.phone = address.phone;
            this.fields.userId = address.userId;
        }
    },
    methods: {
        async save(event) {
            try {
                event.preventDefault();
                this.loading = true;
                const authStore = useAuthStore();

                let response = null;
                let data = this.fields;
                data.userId = authStore.user.id;
                let url = "Address/Create";

                if (this.fields.id && this.fields.id != 0) {
                    url = "Address/Update";
                    response = await fetchWrapper.put(url, data);
                } else {
                    response = await fetchWrapper.post(url, data);
                }
                this.loading = false;
                this.status = 'Updated Successfully';
                this.statusClass = 'success';
            }
            catch (error) {
                this.loading = false;
                this.status = 'Error when updating address';
                this.statusClass = 'danger';
                console.log(error);
            }
        }
    }
}
</script>

<style scoped></style>