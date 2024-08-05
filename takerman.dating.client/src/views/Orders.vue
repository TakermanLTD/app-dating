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
            <tr v-for="(spot, index) in this.savedSpots" :id="'spot_' + spot.id" :key="index">
              <td>{{ spot.id }}</td>
              <td>{{ spot.dateType + ' ' + spot.title + ' за ' + spot.ethnicity }}</td>
              <td>{{ new Date(spot.startsOn).toLocaleDateString() + ' ' + new Date(spot.startsOn).toLocaleTimeString() }}</td>
              <td>{{ spot.menCount }}/{{ spot.minMen }}</td>
              <td>{{ spot.womenCount }}/{{ spot.minWomen }}</td>
              <td>{{ spot.minAges }}-{{ spot.maxAges }} год.</td>
              <td>{{ spot.price }}{{ $t('common.currencySign') }}</td>
              <td><router-link class="btn btn-success" :to="'date?id=' + spot.id">Visit</router-link></td>
              <td><button class="btn btn-warning" @click="this.unsaveSpot(spot.id)">Unsave</button></td>
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
                <button v-if="order.refundable && order.status != 'Canceled'" @click="this.cancel(order.id)" class="btn btn-danger">Cancel</button>
              </td>
            </tr>
          </table>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import breadcrumbs from '../components/Breadcrumbs.vue';
import loader from '../components/Loader.vue';
import card from '../components/Card.vue';
import heading from '../components/Heading.vue';
import { useAuth0 } from '@auth0/auth0-vue';

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
  async mounted() {
    this.loading = true;
    const { user, isAuthenticated } = useAuth0();
    if (isAuthenticated.value) {
      this.orders = await fetch('Order/GetByUserId?userId=' + user.id);
      this.savedSpots = await fetch('Dates/GetSavedSpots?userId=' + user.id);
    }
    this.loading = false;
  },
  methods: {
    async cancel(orderId) {
      await fetch('Order/Cancel?id=' + orderId)
        .then((e) => {
          order.status = 'Отказана';
        });
    },
    async unsaveSpot(dateId) {
      const { user } = useAuth0();
      await fetch('Dates/UnsaveSpot?userId=' + user.id + '&dateId=' + dateId)
        .then((result) => {
          this.emitter.emit('addToSpotCount', { 'eventContent': -1 });
          document.getElementById('spot_' + dateId).remove();
        });
    }
  },
  components: { breadcrumbs, loader, card, heading }
}
</script>

<style scoped></style>