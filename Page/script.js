// ==========================
// Preloader
// ==========================
window.addEventListener('load', () => {
  document.getElementById('preloader').style.display = 'none';
  document.getElementById('content').style.display = 'block';
});

// ==========================
// Scroll reveal sections
// ==========================
const sections = document.querySelectorAll('.section');

const reveal = () => {
  sections.forEach(sec => {
    const top = sec.getBoundingClientRect().top;
    if (top < window.innerHeight - 100) {
      sec.classList.add('visible');
    }
  });
};

window.addEventListener('scroll', reveal);
reveal();

// ==========================
// Load README.md from GitHub
// ==========================
async function loadReadme() {
  const response = await fetch('https://raw.githubusercontent.com/NeleN-Games/Forest-Awakens-Unity-6.2/master/README.md');
  const text = await response.text();
  document.getElementById('readme-content').innerHTML = marked.parse(text);
}

loadReadme();

// ==========================
// Back-to-Top Button
// ==========================
const backToTop = document.getElementById('back-to-top');

window.addEventListener('scroll', () => {
  if (window.scrollY > 80) {
    backToTop.classList.add('show');
  } else {
    backToTop.classList.remove('show');
  }
});

backToTop.addEventListener('click', () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
});
function toggleVideoFullscreen(videoItem, videoElement, exit=false) {
    const isFullScreen = exit ? videoItem.classList.remove('is-fullscreen') : videoItem.classList.toggle('is-fullscreen');
    const body = document.body;

    if (videoItem.classList.contains('is-fullscreen')) {
        const rect = videoItem.getBoundingClientRect();
        const scrollTop = window.scrollY || window.pageYOffset;
        const elementMiddle = rect.top + scrollTop + rect.height / 2;
        const viewportMiddle = window.innerHeight / 2;
        window.scrollTo({
            top: elementMiddle - viewportMiddle,
            behavior: 'smooth'
        });

        videoElement.play();
        body.classList.add('modal-open');
        videoElement.style.cursor = 'pointer';
    } else {
        videoElement.pause();
        body.classList.remove('modal-open');
        videoElement.style.cursor = 'zoom-in';
    }
}

document.querySelectorAll('.video-item').forEach(videoItem => {
    const videoElement = videoItem.querySelector('video');
    videoElement.style.cursor = 'zoom-in';

    videoItem.addEventListener('click', (event) => {
        const isCurrentlyFullscreen = videoItem.classList.contains('is-fullscreen');

        if (isCurrentlyFullscreen && event.target === videoElement) {
            if (videoElement.paused) videoElement.play();
            else videoElement.pause();
        } else {
            toggleVideoFullscreen(videoItem, videoElement);
        }
    });
});

document.addEventListener('click', (event) => {
    const activeFullScreenItem = document.querySelector('.video-item.is-fullscreen');
    if (activeFullScreenItem) {
        const videoElement = activeFullScreenItem.querySelector('video');
        if (!activeFullScreenItem.contains(event.target) || event.target !== videoElement) {
            toggleVideoFullscreen(activeFullScreenItem, videoElement, true);
        }
    }
});

document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape') {
        const activeFullScreenItem = document.querySelector('.video-item.is-fullscreen');
        if (activeFullScreenItem) {
            const videoElement = activeFullScreenItem.querySelector('video');
            toggleVideoFullscreen(activeFullScreenItem, videoElement, true);
        }
    }
});
