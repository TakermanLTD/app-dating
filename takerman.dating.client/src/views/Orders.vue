<template>
  <breadcrumbs :paths="breadcrumbs" />
  <section class="container">
    <loader v-if="this.loading == true" />
    <div v-else>
      <div class="container">
        <heading heading="Запазени срещи" />
        <div v-if="!this.savedSpots || this.savedSpots.length === 0">
          <p class="text-center">
            Нямате запазени срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div v-else class="card-deck">
          <table class="table table-fluid">
            <thead>
              <tr>
                <th>Number</th>
                <th>Name</th>
                <th>Starts on</th>
                <th>Men</th>
                <th>Women</th>
                <th>Ages</th>
                <th>Price</th>
                <th></th>
              </tr>
            </thead>
            <tr v-for="(spot, index) in this.savedSpots" :id="spot.id" :key="index">
              <td>{{ spot.id }}</td>
              <td>{{ spot.dateType + ' ' + spot.title + ' за ' + spot.ethnicity }}</td>
              <td>{{ new Date(spot.startsOn).toLocaleDateString() + ' ' + new Date(spot.startsOn).toLocaleTimeString() }}</td>
              <td>{{ spot.menCount }}/{{ spot.minMen }}</td>
              <td>{{ spot.womenCount }}/{{ spot.minWomen }}</td>
              <td>{{ spot.minAges }}-{{ spot.maxAges }} год.</td>
              <td>{{ spot.price }}{{ $t('common.currencySign') }}</td>
              <td><router-link class="btn btn-success" :to="'date?id=' + spot.id">Visit</router-link></td>
            </tr>
          </table>
        </div>
        <br />
        <heading heading="Купени срещи" />
        <div v-if="!this.orders || this.orders.length === 0">
          <p class="text-center">
            Не сте се записали за срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div v-else class="card-deck">
          <table class="table table-fluid">
            <thead>
              <tr>
                <th>Number</th>
                <th>Name</th>
                <th>Starts on</th>
                <th>Men</th>
                <th>Women</th>
                <th>Ages</th>
                <th>Total</th>
                <th></th>
              </tr>
            </thead>
            <tr v-for="(order, index) in this.orders" :id="order.id" :key="index">
              <td>{{ order.id }}</td>
              <td>{{ order.date.dateType + ' ' + order.date.title + ' за ' + order.date.ethnicity }}</td>
              <td>{{ new Date(order.date.startsOn).toLocaleDateString() + ' ' + new Date(order.date.startsOn).toLocaleTimeString() }}</td>
              <td>{{ order.date.menCount }}/{{ order.date.minMen }}</td>
              <td>{{ order.date.womenCount }}/{{ order.date.minWomen }}</td>
              <td>{{ order.date.minAges }}-{{ order.date.maxAges }} год.</td>
              <td>{{ order.total + order.currency }}</td>
              <td>
                <router-link class="btn btn-success" :to="'date?id=' + order.date.id">Visit</router-link>
                <!--<router-link v-if="order.refundable" class="btn btn-danger" :to="'order/refund?id=' + order.id">Refund</router-link>-->
              </td>
            </tr>
          </table>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
import moment from 'moment';
import breadcrumbs from '../components/Breadcrumbs.vue';
import loader from '../components/Loader.vue';
import card from '../components/Card.vue';
import heading from '../components/Heading.vue';

export default {
  data() {
    return {
      orders: null,
      loading: false,
      breadcrumbs: [
        { name: '/', title: 'Начало' },
        { name: 'orders', title: 'Мои срещи' }
      ]
    };
  },
  async created() {
    this.loading = true;
    const authStore = useAuthStore();
    this.orders = await fetchWrapper.get('Order/GetByUserId?userId=' + authStore.user.id);
    this.savedSpots = await fetchWrapper.get('Dates/GetSavedSpots?userId=' + authStore.user.id);
    this.loading = false;
  },
  methods: {
    async cancel(order) {
      await fetchWrapper.put('Order/Cancel?id=' + order.id)
        .then((e) => {
          order.status = 'Отказана';
        });
    }
  },
  components: { breadcrumbs, loader, card, heading }
}
</script>

<style scoped></style>