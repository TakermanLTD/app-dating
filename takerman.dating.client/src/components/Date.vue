<template>
  <section id="breadcrumbs" class="breadcrumbs">
    <div class="container">
      <ol>
        <li><router-link to="/">Начало</router-link></li>
        <li>Среща</li>
      </ol>
    </div>
  </section>
  <section class="container">
    <div v-if="this.loading">Зареждане...</div>
    <div class="row text-center" v-else>
      <div>
        <hgroup>
          <h2 class="text-center">{{ date?.title }}</h2><br />
        </hgroup>
      </div>
      <div class="col card" style="margin: 15px; display: inline-block;">
        <img class="card-img-top" src="/src/assets/img/date/date-thumbnail.jpeg" alt="Date">
        <div class="card-body">
          <h4 class="card-title text-center">{{ date?.title }}</h4>
          <h6 class="card-title text-center">
            за <strong>{{ date?.ethnicity }}</strong> - {{ date?.minAges }}-{{ date?.maxAges }} год.</h6>
          <table class="table">
            <tbody>
              <tr>
                <td>Мъже {{ date?.menCount }}/{{ date?.minMen }}</td>
                <td>Жени {{ date?.womenCount }}/{{ date?.minWomen }}</td>
              </tr>
              <tr>
                <td>{{ date?.startsOn ? moment(date?.startsOn).format("DD MMM, hh:mm") : 'Предстои' }}</td>
                <td>Цена {{ date?.price }}лв.</td>
              </tr>
            </tbody>
          </table>
          <p class="card-text text-center">
            {{ date.description }}
          </p>
          <p v-if="date?.status === 'NotApproved'" class="text-center">
            <a @click="date?.isSpotSaved ? unsaveSpot(date) : saveSpot(date)"
              :class="date?.isSpotSaved ? 'btn btn-danger' : 'btn btn-primary'">
              {{ date?.isSpotSaved ? 'Няма да присъствам' : 'Запази място' }}</a>
          </p>
          <p v-else-if="date?.status === 'Approved'" class="text-center">
            <PayButton v-if="status === 'notBought'" :date-id="date.id" :on-approve="onApprove">Купи</PayButton>
          <div v-else-if="status === 'bought'" class="alert alert-success" role="alert">
            <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
            <span class="sr-only"></span> Закупихте срещата успешно. Можете да я видите от менюто <router-link
              to="orders">'Мои срещи'</router-link>
          </div>
          <div v-else-if="status === 'failed'" class="alert alert-danger" role="alert">
            <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>
            <span class="sr-only"></span> Стана грешка при плащането. Моля опитайте пак или се свържете с нас през
            контактната форма или чата
          </div>
          </p>
          <p v-else-if="date?.status === 'Started'" class="text-center">
            <strong>Срещата е започнала</strong>
          </p>
          <p v-else class="text-center">
            <strong>Срещата е завършила</strong>
          </p>
        </div>
      </div>
      <div style="height: 100px;" class="col">
        <p class="center-text-vertically">
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
  </section>
</template>

<script lang="js">
import moment from 'moment';
import { fetchWrapper, router } from '@/helpers';
import { useAuthStore } from '@/stores';
import PayButton from './PayButton.vue';

export default {
  data() {
    return {
      moment: moment,
      date: null,
      loading: false,
      status: 'notBought'
    }
  },
  components: {
    PayButton
  },
  async created() {
    this.loading = true;
    const authStore = useAuthStore();
    let queryString = window.location.search;
    let urlParams = new URLSearchParams(queryString);

    if (urlParams.has('id')) {
      this.id = urlParams.get('id');
      this.date = await fetchWrapper.get('Dates/Get?id=' + this.id);
    }
    this.loading = false;
  },
  methods: {
    onApprove() {
      this.status = 'bought';
    }
  }
}
</script>

<style scoped>
.center-text-vertically {
  margin-top: 200px;
}
</style>