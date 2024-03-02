<template>
  <breadcrumbs :paths="this.breadcrumbs" />
  <loader v-if="this.loading" />
  <div class="row text-center" v-else>
    <heading :heading="date?.title" />
    <card :id="date?.id" />
    <div style="height: 100px;" class="col">
      <div v-if="date.status === 'Approved'">{{ this.timeRemaining }}</div>
      <div v-else-if="date.status === 'Started'">
        <button class="btn btn-success btn-lg" @click="enterDate">Влез в Срещата</button>
      </div>
      <p class="center-text-vertically">
        {{ date.description }}
        <br />
        <br />
        <br />
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
  <br />
  <div class="row">
    <Choices
      v-if="(date && (date.status == 'Started' || date.status == 'Finished' || date.status == 'ResultsRevealed')) && date?.id != null"
      :date-id="date?.id" />
  </div>
  <br />
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
      timeRemaining: null,
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
      if (new Date(this.date?.startsOn) > new Date()) {
        let countdownToStart = setInterval(async () => {
          let remaining = new Date(this.date?.startsOn);
          let now = new Date();
          this.timeRemaining = moment(remaining - now).utc().format("HH:mm:ss");
          if(new Date(this.date?.startsOn) <= new Date()) {
            this.date.status = 'Started';
            clearInterval(countdownToStart);
            this.date = await fetchWrapper.get('Dates/SetStatus?id=' + this.id + '&status=' + this.date.status);
          }
        }, 1000);
      }
    }
    this.loading = false;
  },
  methods: {
    enterDate() {
      window.open('https://zoom.com', '_blank', 'noreferrer');
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