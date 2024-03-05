<template>
  <breadcrumbs :paths="this.breadcrumbs" />
  <loader v-if="this.loading" />
  <div class="row text-center" v-else>
    <heading :heading="date?.title" />
    <card :id="date?.id" />
    <div style="height: 100px;" class="col">
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