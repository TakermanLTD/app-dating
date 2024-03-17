<template>
    <div class="btn-group">
        <button type="button" class="btn btn-sm btn-light dropdown-toggle" data-toggle="dropdown">{{ selectedCurrency }}</button>
        <div class="dropdown-menu dropdown-menu-right">
            <button @click="changeCurrency(currency)" v-for="(currency, currencyKey) in this.currencies" :key="currencyKey" class="dropdown-item" type="button">{{ currency }}</button>
        </div>
    </div>
</template>

<script lang="js">
import cookies from '../helpers/cookies';

export default {
    data() {
        return {
            selectedCurrency: 'EUR',
            currencies: ['EUR', 'USD', 'RUB', 'LEI', 'LEV']
        }
    },
    mounted() {
        var currencyCookie = cookies.get('currency');
        if (currencyCookie) {
            this.selectedCurrency = currencyCookie;
        } else {
            cookies.set('currency', this.selectedCurrency);
        }
    },
    methods: {
        changeCurrency(currency) {
            this.selectedCurrency = currency;
            cookies.set('currency', currency);
        }
    }
}
</script>