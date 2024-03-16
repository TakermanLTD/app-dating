<template>
    <div class="container-fluid">
        <h2 class="section-title position-relative text-uppercase mx-xl-5 mb-4"><span class="bg-secondary pr-3">{{ $t('contacts.title') }}</span></h2>
        <div class="row px-xl-5">
            <div class="col-lg-7 mb-5">
                <div class="contact-form bg-light p-30">
                    <div id="success"></div>
                    <form name="sentMessage" id="contactForm" novalidate="novalidate">
                        <div class="control-group">
                            <input v-model="fields.name" type="text" class="form-control" id="name" :placeholder="$t('contacts.name')"
                                   required="required" :data-validation-required-message="$t('contacts.name.placeholder')" />
                            <p class="help-block text-danger"></p>
                        </div>
                        <div class="control-group">
                            <input v-model="fields.from" type="email" class="form-control" id="email" :placeholder="$t('contacts.email')"
                                   required="required" :data-validation-required-message="$t('contacts.email.placeholder')" />
                            <p class="help-block text-danger"></p>
                        </div>
                        <div class="control-group">
                            <textarea v-model="fields.body" class="form-control" rows="8" id="message" :placeholder="$t('contacts.message')"
                                      required="required" :data-validation-required-message="$t('contacts.message.placeholder')"></textarea>
                            <p class="help-block text-danger"></p>
                        </div>
                        <div>
                            <button class="btn btn-primary py-2 px-4" type="submit" id="sendMessageButton">{{ $t('contacts.sendButton') }}</button>
                        </div>
                    </form>
                    <div class="my-3">
                        <div v-show="this.loading === true" class="loading" style="display: block;">Зареждане...
                        </div>
                        <div v-show="this.error !== ''" class="error-message" style="display: block;">{{ this.error
                            }}</div>
                        <div v-show="this.success !== ''" class="sent-message" style="display: block;">{{
            this.success }}</div>
                    </div>
                </div>
            </div>
            <div class="col-lg-5 mb-5">
                <div class="bg-light p-30 mb-30">
                    <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d11831.136099109197!2d24.77365535!3d42.154908299999995!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14acd175be91a221%3A0xf1674e2ed139b008!2zVHNlbnRhckl6dG9jaGVuLCDQsdGD0LsuIOKAntCY0LfRgtC-0YfQtdC94oCcIDgwLCA0MDE4IFBsb3ZkaXY!5e0!3m2!1sen!2sbg!4v1710545678375!5m2!1sen!2sbg" style="width: 100%; height: 250px;border:0;" allowfullscreen="" loading="lazy" frameborder="0" aria-hidden="false" tabindex="0" referrerpolicy="no-referrer-when-downgrade"></iframe>
                </div>
                <div class="bg-light p-30 mb-3">
                    <p class="mb-2"><i class="fa fa-map-marker-alt text-primary mr-3"></i>{{ $t('contacts.street') }}</p>
                    <p class="mb-2"><i class="fa fa-envelope text-primary mr-3"></i>{{ $t('contacts.email') }}</p>
                    <p class="mb-2"><i class="fa fa-phone-alt text-primary mr-3"></i>{{ $t('contacts.tel') }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import Heading from '../components/Heading.vue';
import breadcrumbs from '../components/Breadcrumbs.vue';

export default {
    data() {
        return {
            breadcrumbs: [
                {
                    name: 'contacts',
                    title: 'Контакти'
                }
            ],
            fields: {
                name: '',
                from: '',
                body: '',
                subject: 'A new message from sreshti.net'
            },
            loading: false,
            error: '',
            success: ''
        };
    },
    component: {},
    methods: {
        async send(event) {
            try {
                event.preventDefault();
                this.loading = true;
                let response = await fetchWrapper.post('Notification/SendContactUsMessage', this.fields);
                if (!response) {
                    this.loading = false;
                    this.error = '';
                    this.success = 'Message sent successfully. Thank you!';
                }
                else {
                    this.loading = false;
                    this.error = response.body;
                    this.success = '';
                    console.log(JSON.stringify(response.body));
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
    components: { Heading, breadcrumbs }
}
</script>

<style scoped></style>