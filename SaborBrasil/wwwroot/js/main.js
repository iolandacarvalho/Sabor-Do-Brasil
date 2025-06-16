// main.js - Lógica JS extraída do index.html

// Estado de login
let isLoggedIn = localStorage.getItem("isLoggedIn") === "true";
let currentUserId = localStorage.getItem("userId");

// Elementos
const publicationsContainer = document.getElementById("publications-container");
const publicationTemplate = document.getElementById("publication-template");
const loginBtn = document.getElementById("login-btn");
const loginModal = document.getElementById("login-modal");
const totalLikesElement = document.getElementById("total-likes");
const totalDislikesElement = document.getElementById("total-dislikes");
const userProfile = document.getElementById("user-profile");
const userPhoto = document.getElementById("user-photo");
const userName = document.getElementById("user-name");
const userEmail = document.getElementById("user-email");
const profileBtn = document.getElementById("profile-btn");
const logoutBtn = document.getElementById("logout-btn");
const companyLogo = document.getElementById("company-logo");
const userProfileLogo = document.getElementById("user-profile-logo");
const addDishBtn = document.getElementById("add-dish-btn");

// Funções do modal
function showModal() {
  loginModal.style.display = "block";
}

function closeModal() {
  loginModal.style.display = "none";
}

// Fechar modal ao clicar fora
window.onclick = function(event) {
  if (event.target === loginModal) {
    closeModal();
  }
}

// Verificar se usuário já curtiu
async function userLiked(postId, userId) {
  try {
    const res = await fetch(`/api/publicacoes/UserLiked?postId=${postId}&userId=${userId}`);
    if (!res.ok) return false;
    const data = await res.json();
    return data.liked;
  } catch {
    return false;
  }
}

// Verificar se usuário já deu dislike
async function userDisliked(postId, userId) {
  try {
    const res = await fetch(`/api/publicacoes/UserDisliked?postId=${postId}&userId=${userId}`);
    if (!res.ok) return false;
    const data = await res.json();
    return data.disliked;
  } catch {
    return false;
  }
}

// Carregar estatísticas gerais
async function loadGeneralStats() {
  try {
    const likesResponse = await fetch("/api/publicacoes/TotalLikes");
    if (likesResponse.ok) {
      const totalLikes = await likesResponse.json();
      totalLikesElement.textContent = totalLikes.total || 0;
    } else {
      totalLikesElement.textContent = "0";
    }
    const dislikesResponse = await fetch("/api/publicacoes/TotalDislikes");
    if (dislikesResponse.ok) {
      const totalDislikes = await dislikesResponse.json();
      totalDislikesElement.textContent = totalDislikes.total || 0;
    } else {
      totalDislikesElement.textContent = "0";
    }
  } catch (error) {
    totalLikesElement.textContent = "0";
    totalDislikesElement.textContent = "0";
  }
}

// Carregar pratos do backend
async function loadDishes() {
  try {
    const response = await fetch("/api/publicacoes/Listar");
    const pratos = await response.json();
    publicationsContainer.innerHTML = "";
    if (!pratos.length) {
      publicationsContainer.innerHTML = '<div class="loading">Nenhum prato cadastrado.</div>';
      return;
    }
    for (const prato of pratos) {
      await renderPublicationCard(prato);
    }
  } catch (err) {
    publicationsContainer.innerHTML = '<div class="loading">Erro ao carregar pratos.</div>';
  }
}

// Renderizar card da publicação
async function renderPublicationCard(prato) {
  const card = publicationTemplate.content.cloneNode(true);
  const img = card.querySelector(".publication-image");
  const title = card.querySelector(".publication-title");
  const location = card.querySelector(".publication-location");
  const likeAction = card.querySelector(".like-action");
  const dislikeAction = card.querySelector(".dislike-action");
  const commentAction = card.querySelector(".comment-action");
  const likeCount = card.querySelector(".like-count");
  const dislikeCount = card.querySelector(".dislike-count");
  const commentCount = card.querySelector(".comment-count");

  img.src = prato.imagem || "https://via.placeholder.com/600x250?text=Sem+Imagem";
  img.alt = prato.nome;
  title.textContent = prato.nome;
  let locationText = "";
  if (prato.local && prato.local.trim()) locationText += prato.local;
  if (prato.cidade && prato.cidade.trim()) {
    if (locationText) locationText += " - ";
    locationText += prato.cidade;
  }
  if (prato.estado && prato.estado.trim()) {
    if (locationText) locationText += ", ";
    locationText += prato.estado;
  }
  if (!locationText) locationText = "Local não informado";
  location.textContent = locationText;

  try {
    const likesResponse = await fetch(`/api/publicacoes/LikesCount/${prato.idPost}`);
    if (likesResponse.ok) {
      const likesData = await likesResponse.json();
      likeCount.textContent = likesData.likes || 0;
    }
    const dislikesResponse = await fetch(`/api/publicacoes/DislikesCount/${prato.idPost}`);
    if (dislikesResponse.ok) {
      const dislikesData = await dislikesResponse.json();
      dislikeCount.textContent = dislikesData.dislikes || 0;
    }
    const commentsResponse = await fetch(`/api/publicacoes/CommentsCount/${prato.idPost}`);
    if (commentsResponse.ok) {
      const commentsData = await commentsResponse.json();
      commentCount.textContent = commentsData.comments || 0;
    }
  } catch (error) {
    likeCount.textContent = "0";
    dislikeCount.textContent = "0";
    commentCount.textContent = "0";
  }

  if (isLoggedIn) {
    const liked = await userLiked(prato.idPost, currentUserId);
    const disliked = await userDisliked(prato.idPost, currentUserId);
    if (liked) likeAction.classList.add("active");
    if (disliked) dislikeAction.classList.add("active");
  }

  likeAction.addEventListener("click", async (e) => {
    e.stopPropagation();
    if (!isLoggedIn) { showModal(); return; }
    await handleLike(prato.idPost, likeAction, likeCount);
  });
  dislikeAction.addEventListener("click", async (e) => {
    e.stopPropagation();
    if (!isLoggedIn) { showModal(); return; }
    await handleDislike(prato.idPost, dislikeAction, dislikeCount);
  });
  commentAction.addEventListener("click", async (e) => {
    e.stopPropagation();
    if (!isLoggedIn) { showModal(); return; }
    window.location.href = `/uploads/product.html?id=${prato.idPost}`;
  });
  const cardDiv = card.querySelector('.publication-card');
  cardDiv.addEventListener("click", function() {
    window.location.href = `/uploads/product.html?id=${prato.idPost}`;
  });
  publicationsContainer.appendChild(card);
}

// Manipular like
async function handleLike(postId, likeAction, likeCount) {
  try {
    const res = await fetch(`/api/publicacoes/LikePost`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        postId: postId,
        userId: currentUserId,
      }),
    });
    if (!res.ok) throw new Error("Erro ao dar like");
    const data = await res.json();
    likeAction.classList.add("active");
    const currentLikes = parseInt(likeCount.textContent) || 0;
    likeCount.textContent = currentLikes + 1;
    // Remover dislike se presente
    const dislikeButton = likeAction.closest(".publication-card").querySelector(".dislike-action");
    if (dislikeButton) {
      dislikeButton.classList.remove("active");
      const dislikeCount = dislikeButton.querySelector(".dislike-count");
      const currentDislikes = parseInt(dislikeCount.textContent) || 0;
      if (currentDislikes > 0) {
        dislikeCount.textContent = currentDislikes - 1;
      }
    }
  } catch (error) {
    console.error(error);
  }
}

// Manipular dislike
async function handleDislike(postId, dislikeAction, dislikeCount) {
  try {
    const res = await fetch(`/api/publicacoes/DislikePost`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        postId: postId,
        userId: currentUserId,
      }),
    });
    if (!res.ok) throw new Error("Erro ao dar dislike");
    const data = await res.json();
    dislikeAction.classList.add("active");
    const currentDislikes = parseInt(dislikeCount.textContent) || 0;
    dislikeCount.textContent = currentDislikes + 1;
    // Remover like se presente
    const likeButton = dislikeAction.closest(".publication-card").querySelector(".like-action");
    if (likeButton) {
      likeButton.classList.remove("active");
      const likeCount = likeButton.querySelector(".like-count");
      const currentLikes = parseInt(likeCount.textContent) || 0;
      if (currentLikes > 0) {
        likeCount.textContent = currentLikes - 1;
      }
    }
  } catch (error) {
    console.error(error);
  }
}

// Atualizar interface de login
function updateLoginInterface() {
  isLoggedIn = localStorage.getItem("isLoggedIn") === "true";
  currentUserId = localStorage.getItem("userId");
  if (isLoggedIn) {
    if (loginBtn) loginBtn.style.display = "none";
    if (userProfile) userProfile.style.display = "block";
    if (addDishBtn) addDishBtn.style.display = "block";
    // Exibir foto de perfil no logo
    const userPhotoValue = localStorage.getItem("userPhoto");
    if (companyLogo) companyLogo.style.display = "none";
    if (userProfileLogo) {
      userProfileLogo.src = userPhotoValue || "https://via.placeholder.com/180x180?text=Sem+Foto";
      userProfileLogo.style.display = "block";
    }
    // Exibir nome do usuário na área de perfil
    const userNameValue = localStorage.getItem("userName") || "";
    if (userEmail) userEmail.textContent = userNameValue;
    // Botão perfil
    if (profileBtn) profileBtn.onclick = function() {
      window.location.href = "/perfil.html";
    };
    // Botão logout
    if (logoutBtn) logoutBtn.onclick = function() {
      localStorage.removeItem("isLoggedIn");
      localStorage.removeItem("userId");
      localStorage.removeItem("userName");
      localStorage.removeItem("userEmail");
      localStorage.removeItem("userPhoto");
      window.location.href = "/index.html";
    };
    // Botão cadastrar prato
    if (addDishBtn) addDishBtn.onclick = function() {
      window.location.href = "/Cadastro de Produto.html";
    };
  } else {
    if (loginBtn) loginBtn.style.display = "block";
    if (userProfile) userProfile.style.display = "none";
    if (addDishBtn) addDishBtn.style.display = "none";
    // Exibir logo da empresa
    if (companyLogo) companyLogo.style.display = "block";
    if (userProfileLogo) userProfileLogo.style.display = "none";
  }
}

window.addEventListener('DOMContentLoaded', () => {
  updateLoginInterface();
  loadGeneralStats();
  loadDishes();
});
window.addEventListener('storage', updateLoginInterface);
