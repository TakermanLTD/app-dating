<template>
  <section id="breadcrumbs" class="breadcrumbs">
    <div class="container">
      <ol>
        <li><router-link to="/">Начало</router-link></li>
        <li>Мои срещи</li>
      </ol>
    </div>
  </section>
  <section class="container">
    <h2 class="text-center">Мои срещи</h2><br />
    <div v-if="this.loading == true">Зареждане...</div>
    <div v-else>
      <div class="container">
        <h2>Запазени срещи</h2>
        <br />
        <div class="row text-center">
          <div class="col"><strong>Заглавие</strong></div>
          <div class="col"><strong>Започва на</strong></div>
          <div class="col"><strong>Жени</strong></div>
          <div class="col"><strong>Мъже</strong></div>
          <div class="col"><strong>Цена</strong></div>
          <div class="col"><strong>Етнос</strong></div>
          <div class="col"><strong></strong></div>
        </div>
        <br />
        <div v-if="!this.savedSpots || this.savedSpots.length === 0">
          <p class="text-center">
            Нямате запазени срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div class="row text-center" v-else v-for="(spot, index) in this.savedSpots" :key="index">
          <div class="col">{{ spot.title }}</div>
          <div class="col">{{ spot.startsOn }}</div>
          <div class="col">{{ spot.womenCount }}</div>
          <div class="col">{{ spot.menCount }}</div>
          <div class="col">{{ spot.price }}лв.</div>
          <div class="col">{{ spot.ethnicity }}</div>
          <div class="col">
            <PayButton :date-id="spot.id" v-if="spot.status == 'Approved'">Купи</PayButton>
          </div>
        </div>
        <br />
        <br />
        <h2>Купени срещи</h2>
        <br />
        <div class="row">
          <div class="col text-left"><strong>Номер</strong></div>
          <div class="col text-center"><strong>Цена</strong></div>
          <div class="col text-center"><strong>Статус</strong></div>
          <div class="col-3 text-center"><strong></strong></div>
        </div>
        <br />
        <div v-if="!this.orders || this.orders.length === 0">
          <p class="text-center">
            Не сте се записали за срещи. Можете да го направите от <router-link to="/">началната страница</router-link>
          </p>
        </div>
        <div v-else v-for="(order, index) in this.orders" :key="index">
          <div class="row">
            <div class="col text-left">{{ order.id }}</div>
            <div class="col text-center">{{ order.total?.toFixed(2) }} {{ order.currency }}</div>
            <div class="col text-center">{{ order.status }}</div>
            <div class="col-3 text-center">
              <table v-if="order.refundable">
                <tr>
                  <td>
                    <button class="btn btn-secondary" @click="this.cancel(order)">Отказ</button>
                  </td>
                </tr>
              </table>
            </div>
          </div>
        </div>
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
      orders: null,
      loading: false
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
  components: { PayButton }
}
</script>

<style scoped></style>