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
          <card v-for="(spot, index) in this.savedSpots" :date="spot.id" :key="index" />
        </div>
        <br />
        <heading heading="Купени срещи" />
        <div v-if="!this.orders || this.orders.length === 0">
          <p class="text-center">
            Не сте се записали за срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div v-else class="card-deck">
          <card class="gray-100" v-for="(order, index) in this.orders" :id="order.dateId" :key="index" />
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';
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