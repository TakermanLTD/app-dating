<template>
    <div>
        <h5 class="text-secondary text-uppercase mb-4">{{ $t('footer.newsletter.title') }}</h5>
        <p>{{ $t('footer.newsletter.intro') }}</p>
        <form id="newsletterForm" @submit="this.subscribe">
            <div class="input-group">
                <input v-model="this.fields.email" class="form-control" type="email" name="email" required :placeholder="$t('footer.newsletter.inputPlaceholder')">
                <div class="input-group-append">
                    <button type="submit" class="btn btn-primary">{{ $t('footer.newsletter.buttonText') }}</button>
                </div>
            </div>
        </form>
        <div class="my-3">
            <div v-show="this.loading === true" class="loading" style="display: block;">{{ $t('common.loading') }}...</div>
            <div v-show="this.error !== ''" class="error-message" style="display: block;">{{ this.error }}</div>
            <div v-show="this.success !== ''" class="sent-message" style="display: block;">{{ this.success }}</div>
        </div>
        <h6 class="text-secondary text-uppercase mt-4 mb-3">{{ $t('footer.social.title') }}</h6>
        <div class="d-flex">
            <a class="btn btn-primary btn-square mr-2" target="_blank" :href="$t('footer.social.facebook')"><i class="fab fa-facebook-f"></i></a>
            <a class="btn btn-primary btn-square" target="_blank" :href="$t('footer.social.instagram')"><i class="fab fa-instagram"></i></a>
        </div>
    </div>
</template>

<script lang="js">

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

                let response = await fetch('Notification/SubscripeToNewsletter', this.fields);

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