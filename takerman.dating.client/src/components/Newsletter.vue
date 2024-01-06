<template>
    <div class="col-lg-4 col-md-6 footer-newsletter">
        <h4>Получавай новини</h4>
        <p>Въведете имейла си ако желаете да получавате новини</p>
        <form id="newsletterForm" @submit="this.subscribe">
            <input v-model="this.fields.email" type="email" name="email" required>
            <input type="submit" value="Запиши ме">
        </form>
        <div class="my-3">
            <div v-show="this.loading === true" class="loading" style="display: block;">Зареждане...</div>
            <div v-show="this.error !== ''" class="error-message" style="display: block;">{{ this.error
            }}</div>
            <div v-show="this.success !== ''" class="sent-message" style="display: block;">{{
                this.success }}</div>
        </div>
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';

export default {
    data() {
        return {
            fields: {
                email: null
            },
            loading: false,
            error: '',
            success: ''
        }
    },
    methods: {
        async subscribe(e) {
            try {
                e.preventDefault();
                this.loading = true;

                let response = await fetchWrapper.post('Notification/SubscripeToNewsletter', this.fields);

                if (response == '') {
                    this.loading = false;
                    this.error = '';
                    this.success = 'Записахте се за новините успешно!';
                } else {
                    this.loading = false;
                    this.error = 'Неусшено записване за новини. Моля уведомете ни за грешката.';
                    this.success = '';
                    console.log(JSON.stringify(response.body));
                }

                let newsletter = document.getElementById('newsletterForm');
                if (newsletter) {
                    this.fields.email = null;
                    newsletter.reset();
                }
            }
            catch (error) {
                this.loading = false;
                this.success = '';
                this.error = error;
                console.log(error);
            }
        }
    },
}
</script>

<style scoped></style>