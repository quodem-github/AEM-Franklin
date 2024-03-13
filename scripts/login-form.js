document.body.addEventListener('submit', (e) => {
  if (!e.target.matches('.login-form form')) {
    return;
  }
  e.preventDefault();
  const emailInput = e.target.querySelector('#form-email').value;
  const passwordInput = e.target.querySelector('#form-password').value;
  if (emailInput === '' || passwordInput === '') {
    console.error('Username and password are required');
    return;
  }
  const loginData = {
    email: emailInput,
    password: passwordInput,
  };
  console.log(loginData);
  // Esta es una API para hacer pruebas de login
  fetch('https://reqres.in/api/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(loginData),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then((data) => {
      if (data.token) {
        console.log('Login successful');
        document.cookie = 'userIsLoggedIn=true; path=/';
        window.location.href = '/next-page';
      } else {
        console.error('Login failed');
      }
    })
    .catch((error) => {
      console.error('There has been a problem with your fetch operation: ', error);
    });
});

const isUserLogged = () => {
  if(window.location.pathname !== '/' && !document.cookie.includes('userIsLoggedIn=true')){
    window.location.href = '/';
  }
}

isUserLogged();
