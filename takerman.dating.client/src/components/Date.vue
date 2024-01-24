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
    <div v-else>
      <h2 class="text-center">{{ date?.title }}</h2><br />
      <div class="card" style="margin: 15px; width: 18rem; display: inline-block;">
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
            {{ date.shortDescription }}
          </p>
          <p v-if="date?.status === 'NotApproved'" class="text-center">
            <a @click="date?.isSpotSaved ? unsaveSpot(date) : saveSpot(date)"
              :class="date?.isSpotSaved ? 'btn btn-danger' : 'btn btn-primary'">
              {{ date?.isSpotSaved ? 'Няма да присъствам' : 'Запази място' }}</a>
          </p>
          <p v-else-if="date?.status === 'Approved'" class="text-center">
            <a @click="buy(date)" class="btn btn-success">Купи достъп</a>
          </p>
          <p v-else-if="date?.status === 'Started'" class="text-center">
            <strong>Срещата е започнала</strong>
          </p>
          <p v-else class="text-center">
            <strong>Срещата е завършила</strong>
          </p>
        </div>
        <p>
          {{ date?.description }}
        </p>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import moment from 'moment';
import { fetchWrapper, router } from '@/helpers';
import { useAuthStore } from '@/stores';

export default {
  data() {
    return {
      moment: moment,
      date: null,
      loading: false
    }
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
  }
}
</script>

<style scoped></style>