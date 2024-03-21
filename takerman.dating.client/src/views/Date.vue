<template>
  <breadcrumbs :paths="this.breadcrumbs" />
  <loader v-if="this.loading" />
  <div v-else-if="this.date" class="container-fluid pb-5">
    <div class="row px-xl-5">
      <div class="col-lg-5 mb-30">
        <div id="product-carousel" class="carousel slide" data-ride="carousel">
          <div class="carousel-inner bg-light">
            <div v-for="(picture, pictureKey) in this.date.pictures" :key="pictureKey" class="carousel-item">
              <img class="w-100 h-100" :src="picture.url" alt="Image">
            </div>
          </div>
          <a class="carousel-control-prev" href="#product-carousel" data-slide="prev">
            <i class="fa fa-2x fa-angle-left text-dark"></i>
          </a>
          <a class="carousel-control-next" href="#product-carousel" data-slide="next">
            <i class="fa fa-2x fa-angle-right text-dark"></i>
          </a>
        </div>
      </div>

      <div class="col-lg-7 h-auto mb-30">
        <div class="h-100 bg-light p-30">
          <h3>{{ this.date.title }}</h3>
          <div class="d-flex mb-3">
            <div class="text-primary mr-2">
              <small class="fas fa-star"></small>
              <small class="fas fa-star"></small>
              <small class="fas fa-star"></small>
              <small class="fas fa-star-half-alt"></small>
              <small class="far fa-star"></small>
            </div>
          </div>
          <h3 class="font-weight-semi-bold mb-4">{{ this.date.price }}</h3>
          <p class="mb-4">{{ this.date.shortDescription }}</p>
          <div class="d-flex mb-7">
            <div>
              <div v-if="date.status === 'Approved'">
                Време до започване <br />
                <strong>{{ this.startTime }}</strong> часа
              </div>
              <div v-else-if="date.status === 'Started'">
                <button class="btn btn-success btn-lg" @click="enterDate">Влез в Срещата</button>
                До разкриване на резултатите <br />
                <strong>{{ this.revealTime }}</strong> часа
              </div>
              <div v-else-if="date.status === 'Finished'">
                До разкриване на резултатите <br />
                <strong>{{ this.revealTime }}</strong> часа
              </div>
            </div>
          </div>
          <div class="d-flex align-items-center mb-4 pt-2">
            <p v-if="this.date?.status === 'NotApproved'" class="text-center">
              <a @click="saveSpot(this.date)" class="btn btn-primary">Запази място</a>
            </p>
            <p v-else-if="this.date?.status === 'SavedSpot'" class="text-center">
              <a @click="unsaveSpot(this.date)" class="btn btn-danger">Няма да присъствам</a>
            </p>
            <div v-else-if="this.date?.status === 'Approved'" class="text-center">
              <PayButton v-if="this.path === '/date' && this.date?.price > 0" :date-id="this.date.id"
                         :on-approve="onApprove" :on-error="onError" class="pay-button">Купи</PayButton>
              <router-link v-else class="btn btn-success" :to="'date?id=' + this.date.id + ''">Купи
                срещата</router-link>
              <div v-if="this.paymentStatus === 'success'" class="alert alert-success" role="alert">
                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                <span class="sr-only"></span> Закупихте срещата успешно. Можете да я видите от менюто <router-link
                             to="orders">'Мои срещи'</router-link>
              </div>
              <div v-else-if="this.paymentStatus === 'failed'" class="alert alert-danger" role="alert">
                <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
                <span class="sr-only"></span> Стана грешка при плащането. Моля опитайте пак или се свържете с нас през
                контактната форма или чата
              </div>
            </div>
            <p v-else-if="this.date?.status === 'Bought'" class="text-center">
              <strong>Закупили сте тази среща</strong>
            </p>
            <p v-else-if="this.date?.status === 'Started'" class="text-center">
              <strong>Срещата е започнала</strong>
            </p>
            <p v-else class="text-center">
              <strong>Срещата е завършила</strong>
            </p>
          </div>
          <div class="d-flex pt-2">
            <strong class="text-dark mr-2">{{ $t('social.shareOn') }}:</strong>
            <div class="d-inline-flex">
              <a class="text-dark px-2" :href="$t('social.facebook.url')">
                <i class="fab fa-facebook-f"></i>
              </a>
              <a class="text-dark px-2" :href="$t('social.instagram.url')">
                <i class="fab fa-twitter"></i>
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row px-xl-5">
      <div class="col">
        <div class="bg-light p-30">
          <h4 class="mb-3">{{ $t('date.description.title') }}</h4>
          <Choices
                   v-if="(date && (date.status == 'Started' || date.status == 'Finished' || date.status == 'ResultsRevealed')) && date?.id != null"
                   :date-id="date?.id" />
          <p>{{ $t('date.description') }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="js">
import moment from 'moment';
import { useAuthStore } from '@/stores';
import PayButton from '../components/PayButton.vue';
import { router } from '@/helpers';
import { fetchWrapper } from '@/helpers';
import breadcrumbs from '../components/Breadcrumbs.vue';
import loader from '../components/Loader.vue';
import heading from '../components/Heading.vue';
import Choices from '../components/Choices.vue';

export default {
  data() {
    return {
      id: 0,
      userId: null,
      date: null,
      startTime: null,
      revealTime: null,
      loading: false,
      moment: moment,
      paymentStatus: '',
      breadcrumbs: [
        {
          name: '/',
          title: 'Начало'
        },
        {
          name: '#',
          title: 'Среща'
        }
      ]
    };
  },
  async created() {
    this.loading = true;
    let queryString = window.location.search;
    let urlParams = new URLSearchParams(queryString);

    const authStore = useAuthStore();
    if (authStore.user)
      this.userId = authStore.user.id;
    if (urlParams.has('id')) {
      this.id = urlParams.get('id');
      this.date = await fetchWrapper.get('Dates/Get?id=' + this.id + '&userId=' + this.userId);

      let startsOn = new Date(this.date?.startsOn);
      let countdownToStart = setInterval(async () => {
        switch (this.date.status) {
          case 'Approved':
            this.startTime = moment(startsOn - new Date()).utc().format("HH:mm:ss");
            break;

          case 'Started':
          case 'Finished':
            let reveal = new Date(new Date(startsOn).setDate(startsOn.getDate() + 1));
            this.revealTime = moment(reveal - new Date()).utc().format("HH:mm:ss");
            if (reveal <= new Date()) {
              clearInterval(countdownToStart);
              await this.revealResults();
            }
            break;

          default:
            this.startTime = '';
            this.revealTime = '';
            break;
        }
      }, 1000);
    }
    this.loading = false;
  },
  methods: {
    enterDate() {
      window.open(this.date.videoLink, '_blank', 'noreferrer');
    },
    async revealResults() {
      this.date = await fetchWrapper.post('Dates/SetStatus', { id: this.id, status: 'ResultsRevealed' });
      this.date.status = 'ResultsRevealed';
    },
    async onApprove(e, o) {
      this.paymentStatus = 'success';

      const data = {
        dateId: this.date?.id,
        userId: this.userId,
        paymentId: e.paymentID,
        payerId: e.payerID,
        orderId: e.orderID,
        paymentSource: e.paymentSource
      }

      await fetchWrapper.post('Order/Create', data);

      const payButton = document.getElementsByClassName('pay-button');
      for (let i = 0; i < payButton.length; i++) {
        const element = payButton[i];
        element.style.display = 'none';
      }
    },
    onError(e) {
      this.paymentStatus = 'fail';
    },
    async saveSpot(date) {
      const authStore = useAuthStore();
      if (!authStore.user) {
        router.push('/login?returnUrl=/date?id=' + this.date?.id);
      } else {
        await fetchWrapper.get('Dates/SaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + this.date.id))
          .then((result) => {
            this.date.status = result.status;
            this.date.menCount = result.menCount;
            this.date.womenCount = result.womenCount;
            this.emitter.emit('addToSpotCount', { 'eventContent': 1 });
          });
      }
    },
    async unsaveSpot(date) {
      const authStore = useAuthStore();
      if (!authStore.user) {
        router.push('/login?returnUrl=/date?id=' + this.date?.id);
      } else {
        await fetchWrapper.get('Dates/UnsaveSpot' + (authStore.user == null ? '' : '?userId=' + authStore.user.id + '&dateId=' + this.date.id))
          .then((result) => {
            this.date.status = result.status;
            this.date.menCount = result.menCount;
            this.date.womenCount = result.womenCount;
            this.emitter.emit('addToSpotCount', { 'eventContent': -1 });
          });
      }
    }
  },
  components: {
    loader,
    breadcrumbs,
    heading,
    Choices,
    PayButton
  }
}
</script>

<style scoped>
.center-text-vertically {
  margin-top: 200px;
}
</style>