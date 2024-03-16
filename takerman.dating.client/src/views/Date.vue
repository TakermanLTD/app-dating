<template>
  <breadcrumbs :paths="this.breadcrumbs" />
  <loader v-if="this.loading" />

  <div class="container-fluid pb-5">
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
            <button class="btn btn-primary px-3"><i class="fa fa-shopping-cart mr-1"></i> Buy</button>
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
          <p>
            Бързите срещи са формализиран метод за запознанства лице-в-лице, насърчаващ хора да се запознаят с голям брой
            непознати. <br /> <br />
            Обикновено при бързите срещи се изисква предварителна регистрация. Мъжете и жените се разменят, за да се срещнат
            с всеки на редица кратки „срещи“, които продължават от 3 до 8 минути в зависимост от решение на организаторите,
            които осъществяват събитието. При повечето организатори жените остават седнали на отделни маси, а мъжете се
            редуват да сядат при тях. В края на всеки интервал от няколко минути организаторът издрънчава със звънец,
            почуква по чаша или дава сигнал със свирка, за да предупреди участниците да се преместят към следващата среща. В
            края на събитието участниците предават на организаторите списък с кого биха желали да се срещнат отново.
            Съществуват и варианти, при които участниците попълват резултатите си онлайн. Ако има съвпадения, участниците
            получават взаимно контактите на другия. По време на първоначалната среща не се разменят контакти с цел да се
            намали напрежението от директното одобряване или отказване на някой от кандидатите.
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="js">
import moment from 'moment';
import { fetchWrapper } from '@/helpers';
import breadcrumbs from '../components/Breadcrumbs.vue';
import loader from '../components/Loader.vue';
import heading from '../components/Heading.vue';
import card from '../components/Card.vue';
import Choices from '../components/Choices.vue';

export default {
  data() {
    return {
      id: 0,
      date: null,
      startTime: null,
      revealTime: null,
      loading: false,
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

    if (urlParams.has('id')) {
      this.id = urlParams.get('id');
      this.date = await fetchWrapper.get('Dates/Get?id=' + this.id);

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
    }
  },
  components: {
    loader,
    breadcrumbs,
    heading,
    card,
    Choices
  }
}
</script>

<style scoped>
.center-text-vertically {
  margin-top: 200px;
}
</style>