<template>
    <div class="container">
        <img :src="this.avatar" class="img" width="150" height="150" />
        <button class="btn btn-primary" @click="this.setDefaultAvatar">Махни профилната</button> <br />
        <form id="uploadForm" @submit="this.upload" method="post" enctype="multipart/form-data">
            <input id="fileUpload" type="file" multiple class="btn btn-primary" />
            <button class="btn btn-success">Качи</button>
        </form>
        <div v-if="this.loading === true">Зарежда...</div>
        <div class="pull-left" v-if="this.pictures && this.pictures.length > 0"
            v-for="(picture, pictureKey) in  this.pictures " :key="pictureKey">
            <img :src="picture.url" class="img" :title="'Uploaded on' + picture.uploadOn" width="150" height="150" />
            <button class="btn btn-danger" @click="remove(picture.id)">Изтрий</button>
            <button class="btn btn-primary" @click="setAvatar(picture.url)">Профилна</button>
        </div>
    </div>
</template>
<script>
import Avatar from '../components/Avatar.vue';
export default {
    data() {
        return {
            loading: false,
            userId: null,
            avatar: null,
            pictures: []
        };
    },
    async mounted() {
        const { user } = useAuth0();
        this.userId = user?.id;
        this.pictures = await this.getPictures();
        let result = await fetch('Cdn/GetAvatar?userId=' + this.userId);
        this.avatar = await result.text();
    },
    components: {
        Avatar
    },
    methods: {
        async getPictures() {
            this.loading = true;
            let result = await fetch('Cdn/GetUserPictures?userId=' + this.userId);
            this.loading = false;
            return result;
        },
        async upload(e) {
            this.loading = true;
            e.preventDefault();

            const formData = new FormData();
            const input = document.getElementById('fileUpload').files;
            for (var x = 0; x < input.length; x++) {
                formData.append("files", input[x]);
            }

            await fetch('Cdn/UploadUserPictures?userId=' + this.userId, {
                method: 'POST',
                body: formData
            }).then(response => {
                this.loading = false;
                for (let i = 0; i < response.length; i++) {
                    const picture = response[i];
                    this.pictures[this.pictures.length] = picture;
                }
                location.reload();
            });
        },
        async setAvatar(url) {
            this.avatar = url;
            await fetch('Cdn/SetAvatar?userId=' + this.userId + '&url=' + url);
        },
        async setDefaultAvatar() {
            let result = await fetch('Cdn/SetDefaultAvatar?userId=' + this.userId);
            this.avatar = await result.text();
        },
        async remove(id) {
            await fetch('Cdn/DeletePicture?id=' + id)
                .then(async (response) => {
                    for (let i = 0; i < this.pictures.length; i++) {
                        const picture = this.pictures[i];
                        if (picture.id === id) {
                            if (this.avatar == picture.url) {
                                await this.setDefaultAvatar();
                            }
                            this.pictures.splice(i, 1);
                        }
                    }
                });
        }
    }
};
</script>