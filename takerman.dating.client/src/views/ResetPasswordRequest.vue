<template>
    <div class="container">
        <Heading heading="Възстановяване на парола" />
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
                </div>
                <div class="form-group row">
                    <div class="col-sm-2">
                        <button id="btnSubmit" type="submit" class="btn btn-success text-center">Възстанови
                            паролата</button>
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
</template>

<script lang="js">
import { useAuthStore } from '@/stores';
import Heading from '../components/Heading.vue';

export default {
    data() {
        return {
            fields: {
                email: ''
            },
            status: '',
            statusClass: ''
        };
    },
    methods: {
        async submit(event) {
            try {
                event.preventDefault();
                const authStore = useAuthStore();
                const result = await authStore.resetPassword(this.fields.email)
                    .catch(error => console.log(error));
                if (!result) {
                    this.status = "Не съществува потребител със такъв имейл";
                    this.statusClass = "danger";
                }
                else {
                    this.status = "Изпратихме имейл със линк за възстановяване на паролата. Моля проверете пощата си";
                    this.statusClass = "success";
                }
            }
            catch (error) {
                console.log(error);
                this.status = 'Възникна грешка. Най-вероятно грешката е при нас. Моля уведомете ни за това';
                this.statusClass = 'danger';
            }
        }
    },
    components: { Heading }
}
</script>

<style scoped></style>