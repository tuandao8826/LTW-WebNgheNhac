const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

// header
const userBtn = $('.header__user');
const containerHeader = $('.container__header');
const navItemLinks = $$('.navbar__item a');
const navbarItemSearch = $(".navbar__item-search");

// search
const search = $('.header__search');

// audio
const audio = $("#audio");
const playBtn = $('.audio__control-btn-play');
const progress = $('.audio__control-progress');
const nextBtn = $('.audio__control-btn-next');
const repeatBtn = $('.audio__control-btn-repeat');
const prevBtn = $('.audio__control-btn-prev');
const randomBtn = $('.audio__control-btn-random');
const audioImg = $('.audio__song-info-img');
const audioName = $('.audio__song-info-title');
const audioSinger = $('.audio__song-info-name');
const songPlayer = $$('.list__item-play');
const volume = $('.audio__setting-volume-progress');
const volumeContainer = $('.audio__setting-volume');
const volumesBtn = $$('.audio__setting-volume > .audio__setting-icon');

// list song
const songListPlayer = $$('.songs-list__container-item-play');
const songListPlayBtn = $$('.songs-list__container-item-play');
const songListPauseBtn = $$('.songs-list__container-item-play');

// my playlist
const playlist = $('.navbar__item-playlist');
const editBtn = $('.playlist__info-btn-edit');
const edit = $('.eidt__myplaylist');
const close = $('.myplaylist__title i');

const home = {
    isOpenVolume: false,
    isPlaying: false,
    isRepeat: false,
    isRandom: false,
    volumeValue: 100,
    currentIndex: 0,
    currentItemSong: null,

    songs: [],

    song: function (TenBaiHat, TenCaSi, LinkBaiHat, HinhBaiHat) {
        this.TenBaiHat = TenBaiHat;
        this.TenCaSi = TenCaSi;
        this.LinkBaiHat = LinkBaiHat;
        this.HinhBaiHat = HinhBaiHat;
    },

    addSong: function (itemSong) {
        this.currentItemSong = itemSong;
        const tenBaiHat = itemSong.querySelector('#TenBaiHat');
        const tenCaSi = itemSong.querySelector('#TenCaSi');
        const linkBaiHat = itemSong.querySelector('#LinkBaiHat');
        const hinhBaiHat = itemSong.querySelector('#HinhBaiHat');
        const songInfo = new this.song(tenBaiHat.value, tenCaSi.value, linkBaiHat.value, hinhBaiHat.value);
        this.songs.push(songInfo);
    },

    addSongs: function (listSong) {
        const _this = this;
        listSong.forEach(function (item) {
            _this.addSong(item);
        })
    },

    refreshCurrentItemSong: function () {
        const active = $('.list__item-play.active');
        const playing = $('.list__item-play.playing');
        if (active)
            active.classList.remove('active');
        if (playing)
            playing.classList.remove('playing');
    },

    currentSong: function () {
        return this.songs[this.currentIndex];
    },

    handleEvent: function () {
        const _this = this;

        // xử lý khi bấm vào một bài nhạc bất kì
        songPlayer.forEach(function (item, index) {
            item.onclick = function () {
                if (_this.currentItemSong != item) {
                    _this.songs = [];
                    if (_this.currentItemSong != null) {
                        _this.refreshCurrentItemSong();
                    }
                    _this.addSong(item);
                    _this.loadAll();
                }
                else if (_this.songs.length >= 1) {
                    if (item.classList.contains('active')) {
                        audio.pause();
                    } else {
                        audio.play();
                    }
                }
            }
        })

        // xử lý chạy nhạc theo danh sách
        songListPlayer.forEach(function (item, index) {
            item.onclick = function () {
                _this.currentIndex = index;
                if (_this.songs.length == 0) {
                    _this.addSongs(songListPlayer);
                    _this.loadAll();
                } /*else if (item == _this.currentItemSong) {
                    item.classList.remove('active');
                }*/
                else {
                    _this.loadAll();
                }
                _this.currentItemSong = item;
            }
        })

        // xử lý nút active trong thanh navbar bên trái
        window.onload = function () {
            var path = window.location.pathname;
            navItemLinks.forEach(function (item) {
                if (item.pathname == path) {
                    var navbarItem = item.parentNode;
                    $('.navbar__item.active').classList.remove('active');
                    navbarItem.classList.add('active');
                }
            })
        }

        // xử lý khi bấm vào nút tìm kiếm
        navbarItemSearch.onclick = function () {
            search.classList.toggle('active');
        }

        // xử lý khi bấm vào user
        userBtn.onclick = function () {
            userBtn.classList.toggle('active');
        }

        // xử lý thanh trượt đổi màu thanh header
        document.onscroll = function () {
            const scrollTop = window.scrollY || document.documentElement.scrollTop;
            containerHeader.style.backgroundColor = `rgba(0, 0, 0, ${scrollTop / 200}`;
        }

        // xử lý tăng giảm volume
        volume.oninput = function () {
            const changeVolume = volume.value / 100 * 1;
            audio.volume = changeVolume;
            _this.volumeValue = volume.value;
            _this.changeIconVolume();
        }

        // xử lý nút bấm tắt/bật volume
        volumesBtn.forEach(function (item) {
            item.onclick = function () {
                if (!_this.isOpenVolume) {
                    audio.volume = 0;
                    volume.value = 0;
                    _this.isOpenVolume = true;
                } else {
                    audio.volume = _this.volumeValue / 100;
                    volume.value = _this.volumeValue;
                    _this.isOpenVolume = false;
                }
                _this.changeIconVolume();
            }
        })

        // xử lý khi bấm vào nút play
        playBtn.onclick = function () {
            if (_this.isPlaying) {
                audio.pause();
            } else {
                audio.play();
            }
        }

        // xử lý khi song đươc play
        audio.onplay = function () {
            _this.isPlaying = true;
            playBtn.classList.add('active');
            if (_this.currentItemSong != null) {
                _this.currentItemSong.classList.remove('playing');
                _this.currentItemSong.classList.add('active');
            }
        }

        // xử lý khi song pause
        audio.onpause = function () {
            _this.isPlaying = false;
            playBtn.classList.remove('active');
            if (_this.currentItemSong != null) {
                _this.currentItemSong.classList.add('playing');
                _this.currentItemSong.classList.remove('active');
            }
        }

        // khi tiến độ bài hát thay đổi
        audio.ontimeupdate = function () {
            if (audio.duration) {
                const progressPercent = audio.currentTime / audio.duration * 100;
                progress.value = progressPercent;
            }
        }

        // xử lý tự động chuyển bài khi audio ended
        audio.onended = () => {
            if (_this.isRepeat) {
                audio.play();
            } else {
                nextBtn.click(); // tư động click
            }
        }

        // chức năng tua bài hát
        progress.oninput = function () {
            const skipTime = progress.value / 100 * audio.duration;
            audio.currentTime = skipTime;
        }

        //  khi next song
        nextBtn.onclick = () => {
            if (_this.isRandom) {
                _this.playRandomSong();
            } else {
                _this.nextSong();
            }
            _this.loadAll();
        }

        // khi prev song
        prevBtn.onclick = () => {
            if (_this.isRandom) {
                _this.playRandomSong();
            } else {
                _this.prevSong();
            }
            _this.loadAll();
        }

        // xử lý bât / tắt nút random
        randomBtn.onclick = () => {
            if (_this.isRandom) {
                randomBtn.classList.remove('active');
                _this.isRandom = false;
            } else {
                randomBtn.classList.add('active');
                _this.isRandom = true;
            }
        }

        // xử lý bật / tắt nút repeat
        repeatBtn.onclick = () => {
            if (_this.isRepeat) {
                repeatBtn.classList.remove('active');
                _this.isRepeat = false;
            } else {
                repeatBtn.classList.add('active');
                _this.isRepeat = true;
            }
        }

        // xử lý nút bật chỉnh sửa playlist
        editBtn.onclick = function () {
            edit.classList.add('active');
        }

        // xử lý nút tắt chỉnh sửa playlist
        close.onclick = function () {
            edit.classList.remove('active');
        }
    },

    loadCurrentSongList: function () {
        const songListItem = songListPlayer[this.currentIndex];
        const songListItemCurrent = $('.songs-list__container-item-play.active');
        if (songListItem) {
            if (songListItemCurrent)
                songListItemCurrent.classList.remove('active');
            songListItem.classList.add('active');
        }
    },

    loadCurrentSong: function () {
        audioImg.style.backgroundImage = `url(${this.currentSong().HinhBaiHat})`;
        audioName.innerText = this.currentSong().TenBaiHat;
        audioSinger.innerText = this.currentSong().TenCaSi;
        audio.src = this.currentSong().LinkBaiHat;
    },

    loadAll: function () {
        this.loadCurrentSong();
        this.loadCurrentSongList();
        audio.play();
    },

    changeIconVolume: function () {
        if (audio.volume == 0) {
            volumeContainer.classList.add('active');
        } else {
            volumeContainer.classList.remove('active');
        }
    },

    nextSong: function () {
        this.currentIndex++;
        if (this.currentIndex >= this.songs.length) {
            this.currentIndex = 0;
            this.currentItemSong = this.songs[this.currentItemSong];
        }
        if (songListPlayer) {
            this.currentItemSong = songListPlayer[this.currentIndex];
        }
        this.loadCurrentSong();
    },

    prevSong: function () {
        this.currentIndex--;
        if (this.currentIndex < 0) {
            this.currentIndex = this.songs.length - 1;
            this.currentItemSong = this.songs[this.currentItemSong];
        }
        if (songListPlayer) {
            this.currentItemSong = songListPlayer[this.currentIndex];
        }
        this.loadCurrentSong();
    },

    playRandomSong: function () {
        let newIndex;
        do {
            newIndex = Math.floor(Math.random() * this.songs.length);
        } while (this.currentIndex === newIndex);
        this.currentIndex = newIndex;
        if (songListPlayer) {
            this.currentItemSong = songListPlayer[this.currentIndex];
        }
        this.loadCurrentSong();
    },

    start: function () {
        this.handleEvent();
    }
}

home.start();