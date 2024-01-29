<template>
    <div class="container">
        <heading heading="Вход" />
        <br />
        <form @submit="submit">
            <fieldset>
                <div>
                    <div class="form-group row">
                        <label for="email" class="col-sm-2 col-form-label">Имейл</label>
                        <div class="col-sm-10">
                            <input type="email" class="form-control" id="email" required placeholder="Имейл"
                                v-model="fields.email" />
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <label for="password" class="col-sm-2 col-form-label">Парола</label>
                        <div class="col-sm-10">
                            <input type="password" class="form-control" id="password" required placeholder="Парола"
                                v-model="fields.password" />
                        </div>
                    </div>
                    <br />
                </div>
                <div class="form-group row">
                    <div class="col-sm-2">
                        <button id="btnSubmit" type="submit" class="btn btn-success text-center">Вход</button> &nbsp; or
                        <router-link to="/register">Регистрация</router-link> &nbsp;
                    </div>
                    <div class="col-sm-10">
                        <router-link to="/reset-password-request">Въстанови парола</router-link>
                        <div v-if="this.loading">Зареждане...</div>
                        <div v-else v-show="status"
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
</template>

<script lang="js">
import { useAuthStore } from '@/stores';
import heading from '../components/heading.vue';

export default {
    data() {
        return {
            fields: {
                email: '',
                password: ''
            },
            loading: false,
            status: '',
            statusClass: ''
        };
    },
    components: {
        heading
    },
    methods: {
        async submit(event) {
            try {
                event.preventDefault();

                this.loading = true;

                const authStore = useAuthStore();

                const result = await authStore.login(this.fields.email, this.fields.password)
                    .catch(error => console.log(error));

                this.loading = false;

                if (!result) {
                    this.status = "Неуспешно влизане. Моля опитайте пак със други данни за вход";
                    this.statusClass = "danger";
                }
            }
            catch (error) {
                this.loading = false;
                console.log(error);
                this.status = 'Потребителя не съществува. Моля регистрирайте се или ни уведомете.';
                this.statusClass = 'danger';
            }
        }
    }
}
</script>

<style scoped></style>