<template>
    <!-- ======= Contact Section ======= -->
    <section id="contact" class="contact section-bg">
        <div class="container">

            <div class="section-title" data-aos="fade-up">
                <h2>Контакти</h2>
            </div>

            <div class="row">

                <div class="col-lg-5 d-flex align-items-stretch" data-aos="fade-right">
                    <div class="info">
                        <div class="address">
                            <i class="bi bi-geo-alt"></i>
                            <h4>Офис:</h4>
                            <p>бул. Източен 80, Пловдив, България</p>
                        </div>

                        <div class="email">
                            <i class="bi bi-envelope"></i>
                            <h4>Имейл:</h4>
                            <p><a href="mailto:contact@sreshti.net">contact@sreshti.net</a></p>
                        </div>

                        <div class="phone">
                            <i class="bi bi-phone"></i>
                            <h4>Телефон:</h4>
                            <p><a href="tel:+359897887191">+359897887191</a></p>
                        </div>

                        <iframe
                            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d23660.23478328794!2d24.749910999999997!3d42.1603574!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14acd175be91a221%3A0xf1674e2ed139b008!2z0LHRg9C7LiDigJ7QmNC30YLQvtGH0LXQveKAnCA4MCwgNDAxOCBUc2VudGFyLCBQbG92ZGl2!5e0!3m2!1sen!2sbg!4v1700845225077!5m2!1sen!2sbg"
                            frameborder="0" style="border:0; width: 100%; height: 290px;" allowfullscreen loading="lazy"
                            referrerpolicy="no-referrer-when-downgrade"></iframe>
                    </div>
                </div>

                <div class="col-lg-7 mt-5 mt-lg-0 d-flex align-items-stretch" data-aos="fade-left">
                    <div class="php-email-form">
                        <form @submit="send">
                            <div class="row">
                                <div class="form-group col-md-6">
                                    <label for="name">Вашето име</label>
                                    <input type="text" required name="name" class="form-control" id="name" v-model="fields.name" />
                                </div>
                                <div class="form-group col-md-6 mt-3 mt-md-0">
                                    <label for="name">Вашия имейл</label>
                                    <input type="email" required class="form-control" name="email" id="email"
                                        v-model="fields.email" />
                                </div>
                            </div>
                            <!-- <div class="form-group mt-3">
                            <label for="name">Subject</label>
                            <input type="text" class="form-control" name="subject" id="subject" v-model="subject" />
                        </div> -->
                            <div class="form-group mt-3">
                                <label for="name">Съобщение</label>
                                <textarea class="form-control" name="message" rows="10" required
                                    v-model="fields.text"></textarea>
                            </div>
                            <div class="my-3">
                                <div v-show="this.loading === true" class="loading" style="display: block;">Зареждане...</div>
                                <div v-show="this.error !== ''" class="error-message" style="display: block;">{{ this.error
                                }}</div>
                                <div v-show="this.success !== ''" class="sent-message" style="display: block;">{{
                                    this.success }}</div>
                            </div>
                            <div class="text-center"><button type="submit">Изпрати</button></div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </section><!-- End Contact Section -->
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';

export default {
    data() {
        return {
            fields: {
                name: '',
                email: '',
                text: ''
            },
            loading: false,
            error: '',
            success: ''
        };
    },
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
                } else {
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
    }
}
</script>

<style scoped></style>