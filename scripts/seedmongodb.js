const { MongoClient, Binary } = require('mongodb');
const uuid = require('uuid');

const url = 'mongodb://mongoadmin:mongoadmin@localhost:27017';

const repositories = [
  {
    _id: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    Name: 'REPOSITORY 1',
    Description: 'Description for Repository 1',
    Visibility: 0
  },
  {
    _id: new Binary(Buffer.from(uuid.parse('91494575-bff8-4c8e-8dac-8649059835ab')), 3),
    Name: 'REPOSITORY 2',
    Description: 'Description for Repository 2',
    Visibility: 1
  }
];
const milestones = [
  {
    _id: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    Title: 'MILESTONE 1',
    Description: 'Description for Milestone 1',
    DueDate: new Date('2023-12-01T12:00:00.000Z'),
    State: 0,
    RepositoryId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3)
  },
  {
    _id: new Binary(Buffer.from(uuid.parse('91494575-bff8-4c8e-8dac-8649059835ab')), 3),
    Title: 'MILESTONE 2',
    Description: 'Description for Milestone 2',
    DueDate: new Date('2023-12-31T12:00:00.000Z'),
    State: 0,
    RepositoryId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3)
  }
];
const issues = [
  {
    _id: new Binary(Buffer.from(uuid.parse('f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a')), 3),
    Title: 'Issue 1',
    Description: 'Description for Issue 1',
    CreatedAt: new Date('2023-11-01T10:30:00.000Z'),
    Assignee: {
      Email: 'assignee1@example.com'
    },
    Author: {
      Email: 'author1@example.com'
    },
    RepositoryId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    MilestoneId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    TaskType: 1,
    Events: []
  },
  {
    _id: new Binary(Buffer.from(uuid.parse('eafed591-3f43-43f8-b6c0-4ae0fb331cfb')), 3),
    Title: 'Issue 2',
    Description: 'Description for Issue 2',
    CreatedAt: new Date('2023-11-01T14:45:00.000Z'),
    Assignee: {
      Email: 'assignee2@example.com'
    },
    Author: {
      Email: 'author2@example.com'
    },
    RepositoryId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    MilestoneId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    TaskType: 1,
    Events: []
  },
  {
    _id: new Binary(Buffer.from(uuid.parse('b285f7e4-8762-4d33-8391-08ac6a92d97b')), 3),
    Title: 'Issue 3',
    Description: 'Description for Issue 3',
    CreatedAt: new Date('2023-11-01T09:15:00.000Z'),
    Assignee: {
      Email: 'assignee3@example.com'
    },
    Author: {
      Email: 'author3@example.com'
    },
    RepositoryId: new Binary(Buffer.from(uuid.parse('91494575-bff8-4c8e-8dac-8649059835ab')), 3),
    MilestoneId: new Binary(Buffer.from(uuid.parse('91494575-bff8-4c8e-8dac-8649059835ab')), 3),
    TaskType: 1,
    Events: []
  }
];
const branches = [
  {
    _id: new Binary(Buffer.from(uuid.parse('7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23')), 3),
    Name: 'feature-branch-1',
    RepositoryId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3),
    IssueId: new Binary(Buffer.from(uuid.parse('f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a')), 3)
  },
  {
    _id: new Binary(Buffer.from(uuid.parse('b71a920d-dbbf-4a01-8b7d-b9b78413e9c4')), 3),
    Name: 'bugfix-branch-1',
    RepositoryId: new Binary(Buffer.from(uuid.parse('91494575-bff8-4c8e-8dac-8649059835ab')), 3),
    IssueId: new Binary(Buffer.from(uuid.parse('eafed591-3f43-43f8-b6c0-4ae0fb331cfb')), 3)
  },
  {
    _id: new Binary(Buffer.from(uuid.parse('4a2b67b7-1049-40d8-9c84-fc2622ab8197')), 3),
    Name: 'develop',
    RepositoryId: new Binary(Buffer.from(uuid.parse('b0d58598-8410-4fbf-bab8-46eab3afc34e')), 3)
  }
];
const pullRequests = [
  {
    _id: new Binary(Buffer.from(uuid.parse('d1e3a5c6-2a9b-4a6f-97d3-1f38e639e688')), 3),
    Source: new Binary(Buffer.from(uuid.parse('7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23')), 3),
    Target: new Binary(Buffer.from(uuid.parse('4a2b67b7-1049-40d8-9c84-fc2622ab8197')), 3),
    IssueId: new Binary(Buffer.from(uuid.parse('f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a')), 3),
    MilestoneId: new Binary(Buffer.from(uuid.parse('c4f09d2a-0b3a-4dbd-9a26-8a6a982c8123')), 3),
    TaskType: 0,
    Events: []
  }
];
const comments = [
  {
    "_id": new Binary(Buffer.from(uuid.parse('d1e3a5c6-2a9b-4a6f-97d3-1f38e639e688')), 3),
    "DateTimeOccured": new Date('2023-12-01T15:45:00.000Z'),
    "EventType": 2,
    "Content": "This is the content of the comment for Task 1",
    "TaskId": new Binary(Buffer.from(uuid.parse('f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a')), 3)
  },
  {
    "_id": new Binary(Buffer.from(uuid.parse('7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23')), 3),
    "DateTimeOccured": new Date('2023-12-10T08:30:00.000Z'),
    "EventType": 2,
    "Content": "Comment content for Task 2",
    "TaskId": new Binary(Buffer.from(uuid.parse('eafed591-3f43-43f8-b6c0-4ae0fb331cfb')), 3)
  },
  {
    "_id": new Binary(Buffer.from(uuid.parse('b71a920d-dbbf-4a01-8b7d-b9b78413e9c4')), 3),
    "DateTimeOccured": new Date('2023-12-20T12:15:00.000Z'),
    "EventType": 2,
    "Content": "Another comment for Task 3",
    "TaskId": new Binary(Buffer.from(uuid.parse('b285f7e4-8762-4d33-8391-08ac6a92d97b')), 3)
  }
];
const labels = [
  {
    "_id": new Binary(Buffer.from(uuid.parse('91494575-bff8-4c8e-8dac-8649059835ab')), 3),
    "DateTimeOccured": new Date('2023-12-01T15:45:00.000Z'),
    "EventType": 1,
    "Name": "Label 1",
    "Description": "Description for Label 1",
    "Color": "#FFFF00"
  },
  {
    "_id": new Binary(Buffer.from(uuid.parse('92594575-bcc8-4c8e-8dac-8649059835cd')), 3),
    "DateTimeOccured": new Date('2023-12-01T15:45:00.000Z'),
    "EventType": 1,
    "Name": "Label 2",
    "Description": "Description for Label 2",
    "Color": "#0000FF"
  }
];

async function seedData() {
  const client = new MongoClient(url);

  try {
    await client.connect();

    const db = client.db('GitHubSimulator');
    const issueCollection = db.collection('Issue');
    const repoCollection = db.collection('Repository');
    const milestoneCollection = db.collection('Milestone');
    const branchCollection = db.collection('Branch');
    const pullRequestCollection = db.collection('PullRequest');
    const commentCollection = db.collection('Comment');
    const labelCollection = db.collection('Label');
    
    await issueCollection.insertMany(issues);
    await repoCollection.insertMany(repositories);
    await milestoneCollection.insertMany(milestones);
    await branchCollection.insertMany(branches);
    await pullRequestCollection.insertMany(pullRequests);
    await commentCollection.insertMany(comments);
    await labelCollection.insertMany(labels);

    console.log('Data inserted successfully');
  } finally {
    client.close();
  }
}

seedData();
