<template>
    <div class="card-deck">
        <div v-for="date in dates" :key="date.id" class="card" style="margin: 15px; width: 18rem;">
            <img class="card-img-top" src="/src/assets/img/portfolio/portfolio-1.jpg" alt="Card image cap">
            <div class="card-body">
                <h4 class="card-title text-center">{{ date.title }}</h4>
                <h6 class="card-title text-center">
                    <!-- {{ date.ethnicity }} -->
                     За Копанари {{ date.minAges }}-{{ date.maxAges }} год.</h6>
                <table class="table">
                    <tbody>
                        <tr>
                            <td>Мъже {{ date.menCount }}/{{ date.minMen }}</td>
                            <td>Жени {{ date.womenCount }}/{{ date.minWomen }}</td>
                        </tr>
                        <tr>
                            <td>{{ moment(date.startsOn).format("DD MMM, hA") }}</td>
                            <td>Цена {{ date.price }}лв.</td>
                        </tr>
                    </tbody>
                </table>
                <p class="card-text text-center">
                    {{ date.shortDescription }}
                </p>
                <p class="text-center">
                    <a href="#" class="btn btn-primary">Запази място</a>
                </p>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import moment from 'moment';
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';

export default {
    data() {
        return {
            moment: moment,
            dates: []
        }
    },
    async beforeMount() {
        this.dates = await fetchWrapper.get('Dates/GetAll');
    },
}
</script>

<style scoped></style>