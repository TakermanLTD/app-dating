<template>
    <form @submit="save">
        <div class="container">
            <Heading heading="Регистрация" />
            <br />
            <FacebookLogin></FacebookLogin>
            <fieldset>
                <div>
                    <div class="form-group row">
                        <label for="firstName" class="col-sm-2 col-form-label">Първо име</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="firstName" required placeholder="Първо име"
                                v-model="fields.firstName" />
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <label for="lastName" class="col-sm-2 col-form-label">Фамилия</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="lastName" required placeholder="Фамилия"
                                v-model="fields.lastName" />
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <label for="email" class="col-sm-2 col-form-label">Имейл</label>
                        <div class="col-sm-10">
                            <input type="email" class="form-control" id="email" required placeholder="Имейл"
                                v-model="fields.email" />
                        </div>
                    </div>
                    <br />
                    <div class="form-group row">
                        <label for="gender" class="col-sm-2 col-form-label">Пол</label>
                        <div class="col-sm-10">
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input" type="radio" required name="gender" id="genderMan" :value="1"
                                        v-model="fields.gender"> Мъж
                                </label>
                            </div>
                            <div class="form-check form-check-inline">
                                <label class="form-check-label">
                                    <input class="form-check-input" type="radio" required name="gender" id="genderWoman" :value="2"
                                        v-model="fields.gender"> Жена
                                </label>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <hgroup>
                    <h4>Парола</h4>
                </hgroup>
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
                        <button id="btnSubmit" class="btn btn-success text-center">Регистрация</button> &nbsp; or
                        <router-link to="/login">Вход</router-link>
                    </div>
                    <div class="col-sm-10">
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
        </div>
    </form>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import Heading from '../components/Heading.vue';
import FacebookLogin from '../components/FacebookLogin.vue';

export default {
    data() {
        return {
            fields: {
                firstName: '',
                lastName: '',
                email: '',
                password: '',
                confirmPassword: '',
                gender: 1
            },
            loading: false,
            status: '',
            statusClass: '',
            passwordPattern: ".{8,}"
        };
    },
    methods: {
        async save(event) {
            try {
                event.preventDefault();
                this.loading = true;
                if (!new RegExp(this.passwordPattern).test(this.password) || !new RegExp(this.passwordPattern).test(this.confirmPassword)) {
                    this.loading = false;
                    this.status = 'Паролата не е достатъчно силна. Моля въведете по-силна парола';
                    this.statusClass = 'danger';
                    return;
                }
                if (this.fields.confirmPassword !== this.fields.password) {
                    this.loading = false;
                    this.status = 'Паролата не е същата в полето за потвърждение. Въведете същата парола и на двете места';
                    this.statusClass = 'danger';
                    return;
                }
                let response = await fetchWrapper.post('User/Add', this.fields);
                this.loading = false;
                if (response && response.id != 0) {
                    if (response.status == 204) {
                        this.status = 'Съществува потребител със същия имейл. Моля влезте в системата или възстановете паролата си.';
                        this.statusClass = 'danger';
                        return;
                    }
                    this.status = 'Активационен линк е изпратен до имейла ви. Моля отворете пощата си, за да потвърдите акаунта.';
                    this.statusClass = 'success';
                }
                else {
                    this.status = 'Грешка при регистрация на потребител. Моля уведомете ни';
                    this.statusClass = 'danger';
                }
            }
            catch (error) {
                this.loading = false;
                this.status = 'Грешка при регистрация на потребител. Моля уведомете ни за грешката';
                this.statusClass = 'danger';
                console.log(error);
            }
        }
    },
    components: { Heading }
}
</script>

<style scoped></style>