const sectionApi = document.querySelector('.section.insert-from-api.cards-container .cards-wrapper .cards.block');

const getData = async () => {
  try {
    const response = await fetch('https://api.nytimes.com/svc/books/v3/lists/overview.json?api-key=mSjPOCZGSP3RZdeeGfHdDPMcT6PeTXT5');
    const data = await response.json();
    const books = data.results.lists[0].books.map((book) => ({
      title: book.title,
      author: book.author,
      cover: book.book_image,
      description: book.description,
    }));
    const ul = document.createElement('ul');
    books.forEach((book) => {
      const li = document.createElement('li');
      li.innerHTML = `
        <div class="cards-card-image">
          <img src="${book.cover}" alt="${book.title}" class="card-image">
        </div>
        <div class="cards-card-body">
          <h4 class="card-title">${book.title}</h4>
          <p class="card-author">${book.author}</p>
          <p class="card-description">${book.description}</p>
        </div>`;
      ul.appendChild(li);
    });
    sectionApi.appendChild(ul);
  } catch (error) {
    sectionApi.innerHTML = '<p>Perdón, algo ha ido mal con sus solicitud</p>';
  }
};

getData();
