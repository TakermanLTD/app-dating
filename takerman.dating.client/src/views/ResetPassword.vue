<template>
    <div class="container">
        <Heading heading="Възстановяване на парола" />
        <div v-if="!this.isRequestValid">
            <strong class="text-center">Линка за възстановяване на паролата е изтекъл. Моля опитайте отново или ни уведомете.</strong>
        </div>
        <div v-else>
            <form @submit="submit">
                <fieldset>
                    <div>
                        <div class="form-group row">
                            <label for="password" class="col-sm-2 col-form-label">Парола</label>
                            <div class="col-sm-10">
                                <input type="password" class="form-control" id="password" required placeholder="Парола"
                                    v-model="fields.password" :pattern="this.passwordPattern" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <label for="confirmPassword" class="col-sm-2 col-form-label">Паролата отново</label>
                            <div class="col-sm-10">
                                <input type="password" class="form-control" id="confirmPassword" required
                                    placeholder="Паролата отново" v-model="fields.confirmPassword"
                                    :pattern="this.passwordPattern" />
                            </div>
                        </div>
                        <br />
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-2">
                            <button id="btnSubmit" type="submit" class="btn btn-success text-center">Изпрати</button>&nbsp; or
                            <router-link to="/login">Вход</router-link>
                        </div>
                        <div class="col-sm-10">
                            <div v-show="status"
                                :class="{ 'alert-success': this.statusClass == 'success', 'alert-danger': this.statusClass == 'danger' }"
                                class="alert" role="alert">
                                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                                <span class="sr-only"></span> {{ status }}
                            </div>
                        </div>
                    </div>
                    <br />
                </fieldset>
            </form>
        </div>
    </div>
</template>

<script lang="js">
import { useAuthStore } from '@/stores';
import Heading from '../components/heading.vue';

export default {
    data() {
        return {
            code: '',
            userId: 0,
            isRequestValid: false,
            fields: {
                password: '',
                confirmPassword: ''
            },
            status: '',
            statusClass: '',
            passwordPattern: ".{8,}"
        };
    },
    created() {
        let queryString = window.location.search;
        let urlParams = new URLSearchParams(queryString);
        if (urlParams.has('code') && urlParams.has('userId')) {
            this.isRequestValid = true;
            this.code = urlParams.get('code');
            this.userId = urlParams.get('userId');
        }
        else {
            this.isRequestValid = false;
        }
    },
    methods: {
        async submit(event) {
            try {
                event.preventDefault();
                if (!new RegExp(this.passwordPattern).test(this.password) || !new RegExp(this.passwordPattern).test(this.confirmPassword)) {
                    this.status = 'Паролата не е достатъчно силна. Моля въведете по-силна парола';
                    this.statusClass = 'danger';
                    return;
                }
                if (this.fields.confirmPassword !== this.fields.password) {
                    this.status = 'Паролата не е същата в полето за потвърждение. Въведете същата парола и на двете места';
                    this.statusClass = 'danger';
                    return;
                }
                const authStore = useAuthStore();
                await authStore.changePassword(Number.parseInt(this.userId), this.fields.password)
                    .then(() => {
                    this.loading = false;
                    this.isRequestValid = true;
                    this.status = "Паролата беше променена успешно. Вече можете да влезете в сайта";
                    this.statusClass = "success";
                })
                    .catch(error => {
                    this.loading = false;
                    this.isRequestValid = false;
                    this.status = "Неусшено възстановяване на парола. Моля опитайте отново или се свържете с нас";
                    this.statusClass = "danger";
                });
            }
            catch (error) {
                this.isRequestValid = false;
                console.log(error);
                this.status = 'Грешка при възстановаване на паролата. Моля уведомете ни за грешката';
                this.statusClass = 'danger';
            }
        }
    },
    components: { Heading }
}
</script>

<style scoped></style>