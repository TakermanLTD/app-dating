<template>
    <hgroup>
        <h2 class="text-center">Admin</h2>
    </hgroup>
    <div>
        <hgroup>
            <h3>Dates</h3>
        </hgroup>
        <div>
            <button @click="saveDates" class="btn btn-success">save</button>
            <button @click="deleteDates" class="btn btn-danger">delete all</button>
            <button @click="resetDates" class="btn btn-danger">reset all</button>
            <button @click="deleteSpots" class="btn btn-danger">delete spots</button>
            <button @click="deleteOrders" class="btn btn-danger">delete orders</button>
        </div>
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
                    <input type="text" @input="event => newDate.videoLink = event.target.value" :v-model="newDate.videoLink" class="form-control" :value="newDate.videoLink" />
                </td>
                <td>
                    <button @click="addDate" class="btn btn-success">add</button>
                </td>
            </tr>
            <tr v-for="(date, dateKey) in this.dates" :key="dateKey" :class="date.statusClass">
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
                    <input type="text" @input="event => date.videoLink = event.target.value" :v-model="date.videoLink" class="form-control" :value="date.videoLink" />
                </td>
                <td>
                    <button @click="saveDate(date)" class="btn btn-success">save</button>
                    <button @click="deleteDate(date)" class="btn btn-danger">delete</button>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <hgroup>
            <h3>Users</h3>
        </hgroup>
        <div>
            <button @click="saveUsers" class="btn btn-success">save</button>
            <button @click="deleteUsers" class="btn btn-danger">delete all</button>
            <button @click="resetUsers" class="btn btn-danger">reset all</button>
        </div>
        <table v-if="this.users" class="table table-striped table-bordered">
            <tr>
                <th>ID</th>
                <th>first name</th>
                <th>last name</th>
                <th>email</th>
                <th>country</th>
                <th>city</th>
                <th>phone</th>
            </tr>
            <tr>
                <td></td>
                <td><input @input="event => newUser.firstName = event.target.value" type="text" :v-model="newUser.firstName" class="form-control" :value="newUser.firstName" /></td>
                <td><input @input="event => newUser.lastName = event.target.value" type="text" :v-model="newUser.lastName" class="form-control" :value="newUser.lastName" /></td>
                <td><input @input="event => newUser.email = event.target.value" type="text" :v-model="newUser.email" class="form-control" :value="newUser.email" /></td>
                <td><input @input="event => newUser.country = event.target.value" type="text" :v-model="newUser.country" class="form-control" :value="newUser.country" /></td>
                <td><input @input="event => newUser.city = event.target.value" type="text" :v-model="newUser.city" class="form-control" :value="newUser.city" /></td>
                <td><input @input="event => newUser.phone = event.target.value" type="text" :v-model="newUser.phone" class="form-control" :value="newUser.phone" /></td>
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
                <td>
                    <button @click="saveUser(user)" class="btn btn-success">save</button>
                    <button @click="deleteUser(user)" class="btn btn-danger">delete</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script lang="js">
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
                gender: 1,
                ethnicity: 1
            }
        };
    },
    async mounted() {
        this.ethnicities = await fetch('Options/GetEthnicities');
        this.dateTypes = await fetch('Options/GetDateTypes');
        this.dates = await fetch('Dates/GetAll');
        for (let i = 0; i < this.dates.length; i++) {
            let currDate = this.dates[i];
            if (currDate.status == 2) {
                currDate.statusClass = 'status-approved';
            } else if (currDate.status == 3) {
                currDate.statusClass = 'status-started';
            } else if (currDate.status == 4){ 
                currDate.statusClass = 'status-finished';
            } else if (currDate.status == 5){ 
                currDate.statusClass = 'status-revealed';
            } else {
                currDate.statusClass = 'status-not-approved';
            }
        }
        this.users = await fetch('User/GetAll');
        this.statuses = await fetch('Options/GetDateStatuses');
    },
    methods: {
        async deleteSpots() {
            if (confirm('are you sure?')) {
                await fetch('Admin/DeleteAllSpots');
                this.dates = await fetch('Dates/GetAll');
            }
        },
        async deleteOrders() {
            if (confirm('are you sure?')) {
                await fetch('Admin/DeleteAllOrders');
                this.dates = await fetch('Dates/GetAll');
            }
        },
        async resetDates() {
            if (confirm('are you sure?')) {
                await fetch('Admin/ResetAllDates');
                this.dates = await fetch('Dates/GetAll');
            }
        },
        async addDate() {
            this.newDate.orders = [];
            this.newDate.attendees = [];
            this.newDate.dateType = 1;
            let result = await fetch('Admin/AddDate', this.newDate);
            this.dates.push(result);
        },
        async saveDate(date) {
            date.orders = [];
            date.attendees = [];
            await fetch('Admin/SaveDate', date);
        },
        async saveDates() {
            for (let i = 0; i < this.dates.length; i++) {
                this.dates[i].orders = [];
                this.dates[i].attendees = [];
            }
            await fetch('Admin/SaveAllDates', this.dates);
        },
        async deleteDate(date) {
            await fetch('Admin/DeleteDate?id=' + date.id);
            let index = this.dates.indexOf(date);
            this.dates.splice(index, 1);
        },
        async deleteDates() {
            if (confirm('are you sure?')) {
                await fetch('Admin/DeleteAllDates');
                this.dates = [];
            }
        },
        async addUser() {
            this.newUser.orders = [];
            this.newUser.choices = [];
            this.newUser.pictures = [];
            var result = await fetch('Admin/AddUser', this.newUser);
            this.users.push(result);
        },
        async saveUser(user) {
            user.orders = [];
            user.choices = [];
            user.pictures = [];
            await fetch('Admin/SaveUser', user);
        },
        async saveUsers() {
            for (let i = 0; i < this.users.length; i++) {
                this.users[i].orders = [];
                this.users[i].choices = [];
                this.users[i].pictures = [];
            }
            await fetch('Admin/SaveAllUsers', this.users);
        },
        async deleteUser(user) {
            await fetch('User/Delete?userId=' + user.id);
            let index = this.users.indexOf(user);
            this.users.splice(index, 1);
        },
        async deleteUsers() {
            if (confirm('are you sure?')) {
                await fetch('Admin/DeleteAllUsers');
                this.users = [];
            }
        },
        async resetUsers() {
            if (confirm('are you sure?')) {
                await fetch('Admin/ResetAllUsers');
                this.users = [];
            }
        }
    },
    components: { VueDatePicker }
}
</script>

<style scoped>
.status-not-approved {
    background-color: white;
}

.status-approved {
    background-color: lightyellow;
}

.status-started {
    background-color: organge;
}

.status-finished {
    background-color: lightblue;
}

.status-revealed {
    background-color: lightgreen;
}
</style>