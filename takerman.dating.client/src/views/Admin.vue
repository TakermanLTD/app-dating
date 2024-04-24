<template>
    <hgroup>
        <h2 class="text-center">Admin</h2>
    </hgroup>
    <div>
        <hgroup>
            <h3>Dates</h3>
        </hgroup>
        <table v-if="this.dates" class="table table-striped table-bordered">
            <tr>
                <th>ID</th>
                <th>title</th>
                <th>starts on</th>
                <th>status</th>
                <th>min men</th>
                <th>men</th>
                <th>min women</th>
                <th>women</th>
                <th>min ages</th>
                <th>max ages</th>
                <th>price</th>
                <th>ethnicity</th>
                <th>link</th>
                <th>action</th>
            </tr>
            <tr>
                <td></td>
                <td> <input @input="event => newDate.title = event.target.value" type="text" :v-model="newDate.title" class="form-control" :value="newDate.title" /></td>
                <td>
                    <VueDatePicker v-model="newDate.startsOn" />
                </td>
                <td>
                    <select v-model="newDate.status" class="form-control" :value="newDate.status">
                        <option v-for="(status, statusKey) in this.statuses" :value="status.key" :key="statusKey" @click="newDate.status = status.key">{{ status.value }}</option>
                    </select>
                </td>
                <td> <input type="number" @input="event => newDate.minMen = event.target.value" style="width: 80px;" :v-model="newDate.minMen" class="form-control" :value="newDate.minMen" /></td>
                <td> <input type="number" @input="event => newDate.menCount = event.target.value" style="width: 80px;" :v-model="newDate.menCount" class="form-control" :value="newDate.menCount" /></td>
                <td> <input type="number" @input="event => newDate.minWomen = event.target.value" style="width: 80px;" :v-model="newDate.minWomen" class="form-control" :value="newDate.minWomen" /></td>
                <td> <input type="number" @input="event => newDate.womenCount = event.target.value" style="width: 80px;" :v-model="newDate.womenCount" class="form-control" :value="newDate.womenCount" /></td>
                <td> <input type="number" @input="event => newDate.minAges = event.target.value" style="width: 80px;" :v-model="newDate.minAges" class="form-control" :value="newDate.minAges" /></td>
                <td> <input type="number" @input="event => newDate.maxAges = event.target.value" style="width: 80px;" :v-model="newDate.maxAges" class="form-control" :value="newDate.maxAges" /></td>
                <td> <input type="number" @input="event => newDate.price = event.target.value" style="width: 80px;" :v-model="newDate.price" class="form-control" :value="newDate.price" /></td>
                <td>
                    <select v-model="newDate.ethnicity" class="form-control" :value="newDate.ethnicity">
                        <option v-for="(ethnicity, ethnicityKey) in this.ethnicities" :value="ethnicity.key" :key="ethnicityKey" @click="newDate.ethnicity = ethnicity.key">{{ ethnicity.value }}</option>
                    </select>
                </td>
                <td>
                    <input type="text" @input="event => newDate.videoLink = event.target.value" :v-model="newDate.videoLink" class="form-control" :value="newDate.videoLink" />
                </td>
                <td>
                    <button @click="addDate" class="btn btn-success">add</button>
                </td>
            </tr>
            <tr v-for="(date, dateKey) in this.dates" :key="dateKey">
                <td>{{ date.id }}</td>
                <td> <input @input="event => date.title = event.target.value" type="text" :v-model="date.title" class="form-control" :value="date.title" /></td>
                <td>
                    <VueDatePicker v-model="date.startsOn" />
                </td>
                <td>
                    <select v-model="date.status" class="form-control" :value="date.status">
                        <option v-for="(status, statusKey) in this.statuses" :value="status.key" :key="statusKey" @click="date.status = status.key">{{ status.value }}</option>
                    </select>
                </td>
                <td> <input type="number" @input="event => date.minMen = event.target.value" style="width: 80px;" :v-model="date.minMen" class="form-control" :value="date.minMen" /></td>
                <td> <input type="number" @input="event => date.menCount = event.target.value" style="width: 80px;" :v-model="date.menCount" class="form-control" :value="date.menCount" /></td>
                <td> <input type="number" @input="event => date.minWomen = event.target.value" style="width: 80px;" :v-model="date.minWomen" class="form-control" :value="date.minWomen" /></td>
                <td> <input type="number" @input="event => date.womenCount = event.target.value" style="width: 80px;" :v-model="date.womenCount" class="form-control" :value="date.womenCount" /></td>
                <td> <input type="number" @input="event => date.minAges = event.target.value" style="width: 80px;" :v-model="date.minAges" class="form-control" :value="date.minAges" /></td>
                <td> <input type="number" @input="event => date.maxAges = event.target.value" style="width: 80px;" :v-model="date.maxAges" class="form-control" :value="date.maxAges" /></td>
                <td> <input type="number" @input="event => date.price = event.target.value" style="width: 80px;" :v-model="date.price" class="form-control" :value="date.price" /></td>
                <td>
                    <select v-model="date.ethnicity" class="form-control" :value="date.ethnicity">
                        <option v-for="(ethnicity, ethnicityKey) in this.ethnicities" :value="ethnicity.key" :key="ethnicityKey" @click="date.ethnicity = ethnicity.key">{{ ethnicity.value }}</option>
                    </select>
                </td>
                <td>
                    <input type="text" @input="event => date.videoLink = event.target.value" :v-model="date.videoLink" class="form-control" :value="date.videoLink" />
                </td>
                <td>
                    <button @click="saveDate(date)" class="btn btn-success">save</button>
                    <button @click="removeDate(date)" class="btn btn-danger">remove</button>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <hgroup>
            <h3>Users</h3>
        </hgroup>
        <table v-if="this.users" class="table table-striped table-bordered">
            <tr>
                <th>ID</th>
                <th>first name</th>
                <th>last name</th>
                <th>email</th>
                <th>country</th>
                <th>city</th>
                <th>phone</th>
                <th>facebook</th>
                <th>instagram</th>
            </tr>
            <tr>
                <td></td>
                <td><input @input="event => newUser.firstName = event.target.value" type="text" :v-model="newUser.firstName" class="form-control" :value="newUser.firstName" /></td>
                <td><input @input="event => newUser.lastName = event.target.value" type="text" :v-model="newUser.lastName" class="form-control" :value="newUser.lastName" /></td>
                <td><input @input="event => newUser.email = event.target.value" type="text" :v-model="newUser.email" class="form-control" :value="newUser.email" /></td>
                <td><input @input="event => newUser.country = event.target.value" type="text" :v-model="newUser.country" class="form-control" :value="newUser.country" /></td>
                <td><input @input="event => newUser.city = event.target.value" type="text" :v-model="newUser.city" class="form-control" :value="newUser.city" /></td>
                <td><input @input="event => newUser.phone = event.target.value" type="text" :v-model="newUser.phone" class="form-control" :value="newUser.phone" /></td>
                <td><input @input="event => newUser.facebook = event.target.value" type="text" :v-model="newUser.facebook" class="form-control" :value="newUser.facebook" /></td>
                <td><input @input="event => newUser.instagram = event.target.value" type="text" :v-model="newUser.instagram" class="form-control" :value="newUser.instagram" /></td>
                <td>
                    <button @click="addUser()" class="btn btn-success">add</button>
                </td>
            </tr>
            <tr v-for="(user, userKey) in this.users" :key="userKey">
                <td>{{ user.id }}</td>
                <td><input @input="event => user.firstName = event.target.value" type="text" :v-model="user.firstName" class="form-control" :value="user.firstName" /></td>
                <td><input @input="event => user.lastName = event.target.value" type="text" :v-model="user.lastName" class="form-control" :value="user.lastName" /></td>
                <td><input @input="event => user.email = event.target.value" type="text" :v-model="user.email" class="form-control" :value="user.email" /></td>
                <td><input @input="event => user.country = event.target.value" type="text" :v-model="user.country" class="form-control" :value="user.country" /></td>
                <td><input @input="event => user.city = event.target.value" type="text" :v-model="user.city" class="form-control" :value="user.city" /></td>
                <td><input @input="event => user.phone = event.target.value" type="text" :v-model="user.phone" class="form-control" :value="user.phone" /></td>
                <td><input @input="event => user.facebook = event.target.value" type="text" :v-model="user.facebook" class="form-control" :value="user.facebook" /></td>
                <td><input @input="event => user.instagram = event.target.value" type="text" :v-model="user.instagram" class="form-control" :value="user.instagram" /></td>
                <td>
                    <button @click="saveUser(user)" class="btn btn-success">save</button>
                    <button @click="removeUser(user)" class="btn btn-danger">remove</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import moment from 'moment';

export default {
    data() {
        return {
            moment: moment,
            ethnicities: [],
            dateTypes: [],
            dates: [],
            users: [],
            statuses: [],
            newDate: {
                title: '',
                startsOn: new Date(),
                status: 1,
                minMen: 0,
                menCount: 0,
                minWomen: 0,
                womenCount: 0,
                minAges: 0,
                maxAges: 0,
                price: 0,
                description: '',
                shortDescription: ''
            },
            newUser: {
                firstName: '',
                lastName: '',
                email: '',
                password: 'Hakerman91!',
                country: '',
                city: '',
                phone: '',
                facebook: '',
                instagram: '',
                gender: 1,
                ethnicity: 1
            }
        };
    },
    async mounted() {
        this.ethnicities = await fetchWrapper.get('Options/GetEthnicities');
        this.dateTypes = await fetchWrapper.get('Options/GetDateTypes');
        this.dates = await fetchWrapper.get('Dates/GetAll');
        this.users = await fetchWrapper.get('User/GetAll');
        this.statuses = await fetchWrapper.get('Options/GetDateStatuses');
    },
    methods: {
        async addDate() {
            this.newDate.orders = [];
            this.newDate.attendees = [];
            let result = await fetchWrapper.post('Dates/Add', this.newDate);
            this.dates.push(result);
        },
        async saveDate(date) {
            date.orders = [];
            date.attendees = [];
            await fetchWrapper.post('Dates/Save', date);
        },
        async removeDate(date) {
            await fetchWrapper.delete('Dates/Delete?id=' + date.id);
        },
        async addUser() {
            this.newUser.orders = [];
            this.newUser.choices = [];
            this.newUser.pictures = [];
            var result = await fetchWrapper.post('User/AdminAdd', this.newUser);
            this.users.push(result);
        },
        async saveUser(user) {
            user.orders = [];
            user.choices = [];
            user.pictures = [];
            await fetchWrapper.put('User/AdminSave', user);
        },
        async removeUser(user) {
            await fetchWrapper.delete('User/Delete?userId=' + user.id);
        }
    },
    components: { VueDatePicker }
}
</script>

<style scoped></style>