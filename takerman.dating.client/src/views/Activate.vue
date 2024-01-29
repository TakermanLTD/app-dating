<template>
    <div class="container">
        <div :class="{ 'alert-success': this.statusClass == 'success', 'alert-danger': this.statusClass == 'danger' }"
            class="alert" role="alert">
            <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
            <span class="sr-only"></span> {{ status }}
        </div>
    </div>
</template>

<script lang="js">
import { useAuthStore } from '@/stores';
import { router } from '@/helpers';

export default {
    data() {
        return {
            code: '',
            userId: 0,
            isRequestValid: false,
            status: '',
            statusClass: '',
            passwordPattern: ".{8,}"
        };
    },
    async created() {
        try {
            let queryString = window.location.search;
            let urlParams = new URLSearchParams(queryString);

            if (urlParams.has('code')) {
                this.code = urlParams.get('code');

                const authStore = useAuthStore();

                await authStore.activate(this.code)
                    .then((result) => {
                        if (!result) {
                            this.status = "Не можем да такъв  потребител. Моля опитайте се отново или ни уведомете за това";
                            this.statusClass = "danger";
                            console.log(error);
                        } else {
                            this.status = 'Активацията е успешна! Ще бъдете прехвърлени към страницата за вход след малко';
                            this.statusClass = "success";
                            setTimeout((tick) => {
                                router.push('/login');
                            }, 5000);
                        }
                    })
                    .catch(error => {
                        this.status = "Не можем да такъв  потребител. Моля опитайте се отново или ни уведомете за това. Най-вероятно е грешка при нас";
                        this.statusClass = "danger";
                        console.log(error);
                    });
            } else {
                this.status = "Не можем да такъв  потребител. Моля опитайте се отново или ни уведомете за това. Най-вероятно е грешка при нас";
                this.statusClass = "danger";
                console.log(error);
            }
        }
        catch (error) {
            console.log(error);
            this.status = 'Възникна грешка при активацията. Моля уведомете ни за грешката чрез формата за контакти';
            this.statusClass = 'danger';
        }
    }
}
</script>

<style scoped></style>