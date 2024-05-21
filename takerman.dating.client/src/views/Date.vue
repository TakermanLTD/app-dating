<template>
  <breadcrumbs :paths="this.breadcrumbs" />
  <loader v-if="this.loading" />
  <div v-else-if="this.date" class="container-fluid pb-5 text-center">
    <div class="row px-xl-5">
      <div class="col-lg-5 mb-30">
        <div id="product-carousel" class="carousel slide" data-ride="carousel">
          <div class="carousel-inner bg-light">
            <div v-for="(picture, pictureKey) in this.date.pictures" :key="pictureKey" style="display: block;" class="carousel-item">
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
          <hgroup>
            <h3>{{ this.date.title }}</h3>
          </hgroup>
          <br />
          <p class="mb-4">{{ $t('date.description') }}</p>
          <p v-if="this.date && this.date.ethnicity" class="mb-4">{{ $t('date.description.' + this.date.ethnicity.toLowerCase()) }}</p>
          <table class="table">
            <tr>
              <td>
                <strong>{{ $t('dates.card.menCount') }}</strong> {{ this.date.menCount }}/{{ this.date.minMen }}
              </td>
              <td>
                <strong>{{ $t('dates.card.womenCount') }}</strong> {{ this.date.womenCount }}/{{ this.date.minWomen }}
              </td>
            </tr>
            <tr>
              <td>
                <strong>{{ $t('dates.card.minAges') }}</strong> {{ this.date.minAges }}
              </td>
              <td>
                <strong>{{ $t('dates.card.maxAges') }}</strong> {{ this.date.maxAges }}
              </td>
            </tr>
            <tr>
              <td>
                <strong>{{ $t('dates.card.ethnicity') }}</strong> {{ this.date.ethnicity }}
              </td>
              <td>
                <strong>{{ $t('dates.card.location') }}</strong> {{ this.date.dateType }}
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <strong>{{ $t('dates.card.startsOn') }}</strong> {{ this.date.startsOn ? this.date.startsOn : 'След запазване на достатъчно места' }}
              </td>
            </tr>
          </table>
          <div>
            <p v-if="this.date.status === 'NotApproved'">
              <a v-if="this.isSpotSaved" @click="unsaveSpot(this.date)" class="btn btn-danger">Няма да присъствам</a>
              <a v-else @click="saveSpot(this.date)" class="btn btn-primary">Запази място</a>
            </p>
            <p v-if="(this.date.status === 'Approved' || this.date.status === 'Finished') && !this.startTime">
              Остава <strong style="font-size: x-large;">{{ this.startTime }}</strong> часа
            </p>
            <div v-if="this.date.status === 'Approved' && this.startTime">
              <PayButton v-if="this.path === '/date' && this.date.price > 0" :date-id="this.date.id" :on-approve="onApprove" :on-error="onError" class="pay-button">Купи</PayButton>
              <router-link v-else class="btn btn-success" :to="'date?id=' + this.date.id + ''">Купи срещата</router-link>
              <br />
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
              <h3 class="font-weight-semi-bold">{{ $t('common.currencySign') }}{{ this.date.price }}</h3>
            </div>
            <div>
              <strong v-if="this.date.status === 'Started'">Срещата е започнала</strong>
            </div>
            <div v-if="this.date.status === 'Finished'">
                Срещата е завършила <br />
                До разкриване на резултатите<br />
              <h3>{{ this.revealTime }}</h3> часа
            </div>
            <div v-if="this.isBought">
              <a :href="this.date.videoLink" class="btn btn-success btn-lg" target="_blank">Влез в срещата</a>
            </div>
          </div>
          <br />
          <div class="d-flex pt-2">
            <strong class="text-dark mr-2">{{ $t('social.shareOn') }}</strong>
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
    <div v-if="this.date && this.date.id != null && (this.date.status == 'Started' || this.date.status == 'Finished' || this.date.status == 'ResultsRevealed')" class="row px-xl-5">
      <div class="col">
        <div class="bg-light p-30">
          <Choices :date-id="this.date.id" />
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
import { useRoute } from 'vue-router';
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
      isBought: false,
      isSpotSaved: false,
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
  computed: {
    path() {
      var route = useRoute();
      return route.path;
    },
    isPast() {
      return new Date(this.date?.startsOn) < new Date();
    }
  },
  async mounted() {
    this.loading = true;
    let queryString = window.location.search;
    let urlParams = new URLSearchParams(queryString);

    const authStore = useAuthStore();
    if (authStore.user)
      this.userId = authStore.user.id;
    if (urlParams.has('id')) {
      this.id = urlParams.get('id');
      this.date = await fetchWrapper.get('Dates/GetDate?id=' + this.id);
      this.isBought = await (await fetch('Dates/IsBought?dateId=' + this.id + '&userId=' + this.userId)).json();
      this.isSpotSaved = await (await fetch('Dates/IsSpotSaved?dateId=' + this.id + '&userId=' + this.userId)).json();

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

      let order = await fetchWrapper.post('Order/Create', data);

      this.date = await fetchWrapper.get('Dates/GetDate?id=' + order.dateId);

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