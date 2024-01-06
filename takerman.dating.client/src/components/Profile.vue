<template>
    <form @submit="save">
        <div class="container">
            <hgroup>
                <h3>Профил</h3>
            </hgroup>
            <br />
            <fieldset>
                <div class="form-group row">
                    <label for="firstName" class="col-sm-2 col-form-label">Първо име</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="firstName" placeholder="Първо име"
                            v-model="fields.firstName" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="lastName" class="col-sm-2 col-form-label">Фамилиия</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="lastName" placeholder="Фамилия"
                            v-model="fields.lastName" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="email" class="col-sm-2 col-form-label">Имейл</label>
                    <div class="col-sm-10">
                        <input type="email" class="form-control" id="email" placeholder="Email" v-model="fields.email" />
                    </div>
                </div>
                <br />
                <hgroup>
                    <h4>Парола</h4>
                </hgroup>
                <div>
                    <div class="form-group row">
                        <label for="password" class="col-sm-2 col-form-label">Парола</label>
                        <div class="col-sm-10">
                            <input type="password" class="form-control" id="password" placeholder="Парола"
                                v-model="fields.password" :pattern="this.passwordPattern" />
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <label for="confirmPassword" class="col-sm-2 col-form-label">Паролата отново</label>
                        <div class="col-sm-10">
                            <input type="password" class="form-control" id="confirmPassword" placeholder="Паролата отново"
                                v-model="fields.confirmPassword" :pattern="this.passwordPattern" />
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <div class="col-sm-2">
                            <button id="btnSubmit" class="btn btn-success text-center">Запиши</button>
                        </div>
                        <div class="col-sm-8">
                            <div v-if="this.loading">Зареждане...</div>
                            <div v-else v-show="status"
                                :class="{ 'alert-success': this.statusClass == 'success', 'alert-danger': this.statusClass == 'danger' }"
                                class="alert" role="alert">
                                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                                <span class="sr-only"></span> {{ status }}
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <button @click="deleteAccount" type="button" id="btnDelete"
                                class="btn btn-danger text-center">Изтрии акаунт</button>
                        </div>
                    </div>
                    <br />
                </div>
            </fieldset>
        </div>
    </form>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';

export default {
    data() {
        return {
            fields: {
                id: 0,
                firstName: '',
                lastName: '',
                email: '',
                password: '',
                confirmPassword: ''
            },
            loading: false,
            status: '',
            statusClass: '',
            passwordPattern: ".{8,}"
        };
    },
    mounted() {
        const authStore = useAuthStore();
        const user = authStore.user;
        this.fields.id = user.id;
        this.fields.firstName = user.firstName;
        this.fields.lastName = user.lastName;
        this.fields.email = user.email;
    },
    methods: {
        async save(event) {
            try {
                event.preventDefault();
                this.loading = true;

                if (this.fields.password && this.fields.password !== '') {
                    if (!new RegExp(this.passwordPattern).test(this.password) || !new RegExp(this.passwordPattern).test(this.confirmPassword)) {
                        this.loading = false;
                        this.status = 'The password confirmation or the password is strong enough. Please use stronger password.';
                        this.statusClass = 'danger';
                        return;
                    }

                    if (this.fields.confirmPassword !== this.fields.password) {
                        this.loading = false;
                        this.status = 'The password confirmation is not the same as the password';
                        this.statusClass = 'danger';
                        return;
                    }
                }

                const response = await fetchWrapper.put("User/Update", this.fields);

                this.loading = false;

                if (response == "" || response.status == 200) {
                    this.status = 'Updated Successfully';
                    this.statusClass = 'success';
                } else {
                    this.status = 'Error when updating user';
                    this.statusClass = 'danger';
                }
            }
            catch (error) {
                this.loading = false;
                this.status = 'Error when updating user';
                this.statusClass = 'danger';
                console.log(error);
            }
        },
        async deleteAccount(event) {
            event.preventDefault();
            if (confirm('Are you sure? All your data and uploads will be deleted if you continue?')) {
                this.loading = true;
                const authStore = useAuthStore();
                await fetchWrapper.delete("User/Delete?userId=" + authStore.user.id);
                this.loading = false;
                authStore.logout();
            }
        }
    }
}
</script>

<style scoped></style>