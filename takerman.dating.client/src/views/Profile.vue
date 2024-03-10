<template>
    <form @submit="save">
        <div class="container">
            <Heading heading="Профил" />
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
                <div class="form-group row">
                    <label for="gender" class="col-sm-2 col-form-label">Пол</label>
                    <div class="col-sm-10">
                        <div class="form-check form-check-inline">
                            <label class="form-check-label">
                                <input class="form-check-input" type="radio" :value="1" name="gender" v-model="fields.gender"> Мъж
                            </label>
                        </div>
                        <div class="form-check form-check-inline">
                            <label class="form-check-label">
                                <input class="form-check-input" type="radio" :value="2" name="gender" v-model="fields.gender"> Жена
                            </label>
                        </div>
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="city" class="col-sm-2 col-form-label">Град</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="city" placeholder="Град" v-model="fields.city" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="country" class="col-sm-2 col-form-label">Държава</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="country" placeholder="Държава"
                            v-model="fields.country" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="phone" class="col-sm-2 col-form-label">Телефон</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="phone" placeholder="Телефон" v-model="fields.phone" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="facebook" class="col-sm-2 col-form-label">Фейсбук</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="facebook" placeholder="Фейсбук"
                            v-model="fields.facebook" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="instagram" class="col-sm-2 col-form-label">Инстаграм</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="instagram" placeholder="Инстаграм"
                            v-model="fields.instagram" />
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label for="ethnicity" class="col-sm-2 col-form-label">Етнос</label>
                    <div class="col-sm-10">
                        <div class="form-group">
                            <select v-model="fields.ethnicity" :value="fields.ethnicity" class="form-control" name="ethnicity" id="ethnicity">
                                <option v-for="(ethnicity, key) in ethnicities" :key="key" :value="ethnicity.key">{{ ethnicity.value }}</option>
                            </select>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <router-link to="/user-gallery"><button class="btn btn-primary text-center">Галерия</button></router-link>
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
import Heading from '../components/Heading.vue';

export default {
    data() {
        return {
            fields: {
                id: 0,
                firstName: '',
                lastName: '',
                email: '',
                gender: 1,
                city: '',
                country: '',
                phone: '',
                facebook: '',
                instagram: '',
                ethnicity: 1,
                password: '',
                confirmPassword: ''
            },
            ethnicities: [],
            loading: false,
            status: '',
            statusClass: '',
            passwordPattern: ".{8,}"
        };
    },
    async beforeCreate() {
        this.ethnicities = await fetchWrapper.get('Options/GetEthnicities');
        const authStore = useAuthStore();
        const data = await fetchWrapper.get('User/Get?id=' + authStore.user.id);
        this.fields.id = data.id;
        this.fields.firstName = data.firstName;
        this.fields.lastName = data.lastName;
        this.fields.email = data.email;
        this.fields.city = data.city;
        this.fields.country = data.country;
        this.fields.gender = data.gender;
        this.fields.phone = data.phone;
        this.fields.facebook = data.facebook;
        this.fields.instagram = data.instagram;
        this.fields.ethnicity = data.ethnicity;
    },
    methods: {
        async save(event) {
            try {
                event.preventDefault();
                this.loading = true;
                if (this.fields.password && this.fields.password !== '') {
                    if (!new RegExp(this.passwordPattern).test(this.password) || !new RegExp(this.passwordPattern).test(this.confirmPassword)) {
                        this.loading = false;
                        this.status = 'Паролата не е достатъчно силна. Моля използвайте по-силна парола';
                        this.statusClass = 'danger';
                        return;
                    }
                    if (this.fields.confirmPassword !== this.fields.password) {
                        this.loading = false;
                        this.status = 'Потвърждението на паролата не е същото. Въведете същата парола в двете полета';
                        this.statusClass = 'danger';
                        return;
                    }
                }
                const response = await fetchWrapper.put("User/Update", this.fields);
                this.loading = false;
                if (response == "" || response.status == 200) {
                    this.status = 'Редактиран успешно';
                    this.statusClass = 'success';
                }
                else {
                    this.status = 'Грешка при редакция на потребител';
                    this.statusClass = 'danger';
                }
            }
            catch (error) {
                this.loading = false;
                this.status = 'Грешка при редакция на потребител. Най-вероятно е от нас. Моля уведомете ни';
                this.statusClass = 'danger';
                console.log(error);
            }
        },
        async deleteAccount(event) {
            event.preventDefault();
            if (confirm('Сигурни ли сте, че искате да изтриете акаунта си?')) {
                this.loading = true;
                const authStore = useAuthStore();
                await fetchWrapper.delete("User/Delete?userId=" + authStore.user.id);
                this.loading = false;
                authStore.logout();
            }
        }
    },
    components: { Heading }
}
</script>

<style scoped></style>