const { MongoClient } = require('mongodb');

const url = 'mongodb://mongoadmin:mongoadmin@localhost:27017'; // Connection URL

const data = [
  {
    _id: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    Title: 'Issue 1',
    Description: 'Description for Issue 1',
    Assigne: 'Assignee 1',
  },
  {
    _id: '91494575-bff8-4c8e-8dac-8649059835ab',
    Title: 'Issue 2',
    Description: 'Description for Issue 2',
    Assigne: 'Assignee 2',
  },
  {
    _id: 'b5291e11-2bdb-481c-bd06-3125260de2bf',
    Title: 'Issue 3',
    Description: 'Description for Issue 3',
    Assigne: 'Assignee 3',
  },
  {
    _id: '2af3edda-14e9-49b0-873b-f9be2c1ee9f4',
    Title: 'Issue 4',
    Description: 'Description for Issue 4',
    Assigne: 'Assignee 4',
  },
  {
    _id: '1dc3200d-eb19-4cfe-beb6-4f0c5bac5be0',
    Title: 'Issue 5',
    Description: 'Description for Issue 5',
    Assigne: 'Assignee 5',
  },
];

async function seedData() {
  const client = new MongoClient(url);

  try {
    await client.connect();

    const db = client.db('GitHubSimulator');
    const collection = db.collection('Issue');

    await collection.insertMany(data);

    console.log('Data inserted successfully');
  } finally {
    client.close();
  }
}

seedData();
